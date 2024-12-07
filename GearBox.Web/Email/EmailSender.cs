using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GearBox.Web.Email;

/*
    Helpful documentation: 
        https://developers.google.com/gmail/api/reference/rest/v1/users.messages
        https://googleapis.dev/dotnet/Google.Apis.Gmail.v1/latest/api/Google.Apis.Gmail.v1.html
*/
public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;
    private readonly EmailConfig _config;
    private static readonly string SERVICE_ACCOUNT_PATH = "./secrets/gmail-service-account.json";
    private static readonly string OAUTH_SECRET_PATH = "./secrets/client_secret.json";

    /// <summary>
    /// If Google API fails do to insufficient scopes, try deleting this file.
    /// </summary>
    private static readonly string OAUTH_TOKEN_PATH = "./secrets/tokens";
    private static readonly IEnumerable<string> SCOPES = [GmailService.Scope.GmailSend];


    public EmailSender(ILogger<EmailSender> logger, IOptions<EmailConfig> config)
    {
        _logger = logger;
        _config = config.Value;
        _config.Validate();
    }


    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var guid = Guid.NewGuid();
        _logger.LogInformation("SendEmailAsync started {Guid}", guid);
        _logger.LogInformation("TO: {Email}", email);
        _logger.LogInformation("SUBJECT: {Subject}", subject);
        _logger.LogInformation("MESSAGE: {HtmlMessage}", htmlMessage);

        if (!_config.SendEmails)
        {
            _logger.LogInformation("Email sending is disabled. Canceling {Guid}", guid);
            return;
        }        



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

        /*
            GMail needs email in RFC2822 format.
            There is no easy way to do this in dotnet.
            Instead, use MailKit.

            MailKit to .eml: https://github.com/jstedfast/MailKit/issues/100
            MailKit documentation: https://mimekit.net/docs/html/R_Project_Documentation.htm
        */
        using var mailkitEmail = new MimeMessage()
        {
            Sender = new MailboxAddress("GearBox", sender),
            Subject = subject,
            Body = new BodyBuilder()
            {
                HtmlBody = htmlMessage
            }.ToMessageBody()
        };
        mailkitEmail.To.Add(new MailboxAddress(email, email));
        var asRfc2822 = Encoding.UTF8.GetBytes(mailkitEmail.ToString());

        var gmailMessage = new Message()
        {
            Raw = Convert.ToBase64String(asRfc2822) 
        };

        using var gmail = new GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = creds
        });
        var request = gmail.Users.Messages.Send(gmailMessage, "me");
        await request.ExecuteAsync();

        _logger.LogInformation("Done sending email {Guid}", guid);
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
}
