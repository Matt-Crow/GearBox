using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace GearBox.Web.Email;

public class EmailSender : IEmailSender
{
    /*
        Helpful documentation: 
            https://developers.google.com/gmail/api/reference/rest/v1/users.messages
            https://googleapis.dev/dotnet/Google.Apis.Gmail.v1/latest/api/Google.Apis.Gmail.v1.html
    */

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Gmail API does not support API keys
        var init = new BaseClientService.Initializer()
        {
            //ApiKey = "AIzaSyAfgIoAUwThwlFjGN5m459EFkMtHwJlpbs"
        };
        
        using var gmail = new GmailService(init);
        var message = new Message
        {
            Payload = new MessagePart()
            {
                Body = new MessagePartBody()
                {
                    Data = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(htmlMessage))
                },
                Headers = []
            }
        };
        message.Payload.Headers.Add(new MessagePartHeader()
        {
            Name = "From",
            Value = "mattcrow19@gmail.com"
        });
        message.Payload.Headers.Add(new MessagePartHeader()
        {
            Name = "To",
            Value = email
        });
        message.Payload.Headers.Add(new MessagePartHeader()
        {
            Name = "Subject",
            Value = subject
        });
        
        var request = gmail.Users.Messages.Send(message, "me");
        var response = await request.ExecuteAsync();
        Console.WriteLine(response.Raw);
    }
}
