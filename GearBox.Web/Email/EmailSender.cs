using System.Net.Mail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace GearBox.Web.Email;

/*
    Helpful documentation: 
        https://developers.google.com/gmail/api/reference/rest/v1/users.messages
        https://googleapis.dev/dotnet/Google.Apis.Gmail.v1/latest/api/Google.Apis.Gmail.v1.html
*/
public class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // load service account credentials
        // Gmail API does not support API keys, so use service account instead
        var creds = ServiceAccountCredential.FromServiceAccountData(File.OpenRead("./secrets/gmail-service-account.json"));
        creds.Scopes = [
            GmailService.Scope.GmailSend // only need this one
        ];
        var init = new BaseClientService.Initializer()
        {
            ApplicationName = "GearBox",
            HttpClientInitializer = creds
        };

        var sender = creds.Id;
        using var mrMime = CreateDotNetEmail(new MailAddress(sender), new MailAddress(email), subject, htmlMessage);

        var asRfc2822 = await ToRFC2822(mrMime);

        // only need to set Raw - that's what the API explorer does
        var message = new Message()
        {
            Raw = Convert.ToBase64String(asRfc2822) 
        };

        using var gmail = new GmailService(init);
        try
        {
            var request = gmail.Users.Messages.Send(message, "me");
            var response = await request.ExecuteAsync();
            Console.WriteLine(response);
        }
        catch (System.Exception ex)
        {
            Console.Error.WriteLine(ex);
            throw;
        }
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
        var smtpFolder = _config.GetValue<string>("SmtpFolder") ?? throw new Exception("Need to set SmtpFolder to a full path to a folder in appsettings");
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
