using System.Net.Mail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace GearBox.Web.Email;

/*
    Helpful documentation: 
        https://developers.google.com/gmail/api/reference/rest/v1/users.messages
        https://googleapis.dev/dotnet/Google.Apis.Gmail.v1/latest/api/Google.Apis.Gmail.v1.html
*/
public class EmailSender : IEmailSender
{
    private readonly EmailConfig _config;
    private static readonly string SERVICE_ACCOUNT_PATH = "./secrets/gmail-service-account.json";
    private static readonly string OAUTH_SECRET_PATH = "./secrets/client_secret.json";

    /// <summary>
    /// If Google API fails do to insufficient scopes, try deleting this file.
    /// </summary>
    private static readonly string OAUTH_TOKEN_PATH = "./secrets/tokens";
    private static readonly IEnumerable<string> SCOPES = [GmailService.Scope.GmailSend];

    public EmailSender(IOptions<EmailConfig> config)
    {
        _config = config.Value;
        _config.Validate();
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        /*
            Google offers three forms of authentication:
            1. API key, which is disabled for GMail
            2. OAuth client ID, which authenticates as a person (not what I want)
            3. Service Account, which keeps failing with 400 error

            I'll keep trying to get the service account to work, but will need to use the OAuth client ID for now.
        */
        IConfigurableHttpClientInitializer creds = _config.UseServiceAccount
            ? await AuthorizeServiceAccount()
            : await AuthorizeOAuthClient();

        var sender = _config.SenderEmailAddress;
        if (_config.UseServiceAccount)
        {
            sender = (await AuthorizeServiceAccount()).Id;
        }

        using var dotnetEmail = CreateDotNetEmail(new MailAddress(sender), new MailAddress(email), subject, htmlMessage);
        var asRfc2822 = await ToRFC2822(dotnetEmail);
        var message = new Message()
        {
            Raw = Convert.ToBase64String(asRfc2822) 
        };

        using var gmail = new GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = creds
        });
        var request = gmail.Users.Messages.Send(message, "me");
        await request.ExecuteAsync();
    }

    private static async Task<UserCredential> AuthorizeOAuthClient()
    {
        /*
            Service account isn't working, so I'm trying this one.
            https://github.com/googleworkspace/dotnet-samples/blob/main/gmail/GmailQuickstart/GmailQuickstart.cs
        */
        using var stream = File.OpenRead(OAUTH_SECRET_PATH);
        var secrets = GoogleClientSecrets.FromStream(stream);
        var creds = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            secrets.Secrets,
            SCOPES,
            "gearbox", // normally, the user's name would go here
            CancellationToken.None,
            new FileDataStore(OAUTH_TOKEN_PATH, true)
        );
        return creds;
    }

    private static async Task<ServiceAccountCredential> AuthorizeServiceAccount()
    {
        using var stream = File.OpenRead(SERVICE_ACCOUNT_PATH);
        var creds = ServiceAccountCredential.FromServiceAccountData(stream);
        creds.Scopes = SCOPES;
        await Task.CompletedTask;
        return creds;
    }

    private static MailMessage CreateDotNetEmail(MailAddress from, MailAddress to, string subject, string htmlBody)
    {
        var result = new MailMessage()
        {
            From = from,
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        result.To.Add(to);
        return result;
    }

    private async Task<byte[]> ToRFC2822(MailMessage email)
    {
        /*
            Need to serialize a MailMessage to RFC 2822 format so gmail can use it.
            Unfortunately, .NET does not make this easy.
            This hacky solution uses SmtpClient to send the email to a file, then reads the file.
            Credit: https://stackoverflow.com/q/18745392
        */

        // we'll temporarily store files here - SmtpClient gives file a random name, so store in unique folder to find it easily
        var smtpFolder = _config.SmtpFolder;
        var uniqueFolderName = Guid.NewGuid().ToString();
        var folderForThisEmail = Path.Combine(smtpFolder, uniqueFolderName);
        Directory.CreateDirectory(folderForThisEmail);

        // sending the email writes it to a file in our folder we just created
        using var smtp = new SmtpClient("localhost")
        {
            DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
            PickupDirectoryLocation = folderForThisEmail
        };
        smtp.Send(email); // send async isn't the async version of this
        
        // we just created this folder, so the only file in there is the email
        var emailFile = Directory.GetFiles(smtp.PickupDirectoryLocation)[0];
        var rfc2822String = await File.ReadAllTextAsync(emailFile);
        Directory.Delete(folderForThisEmail, true);

        var rfc2822Bytes = System.Text.Encoding.UTF8.GetBytes(rfc2822String); 
        return rfc2822Bytes;
    }
}
