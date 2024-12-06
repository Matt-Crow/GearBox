namespace GearBox.Web.Email;

public class EmailConfig
{
    public const string ConfigSection = "Email";

    /// <summary>
    /// If set to true, this will send emails through gmail.
    /// If set to false, this will instead log emails.
    /// </summary>
    public bool SendEmails { get; set; } = false;

    /// <summary>
    /// The email address to use as a sender.
    /// </summary>
    public string SenderEmailAddress { get; set; } = "";

    /// <summary>
    /// Folder where the app will temporarily store emails it sends.
    /// </summary>
    public string SmtpFolder { get; set; } = "";

    /// <summary>
    /// Whether to try using the service account to send emails (currently broken)
    /// </summary>
    public bool UseServiceAccount { get; set; }

    public void Validate()
    {
        if (!SendEmails)
        {
            return;
        }


        if (string.IsNullOrEmpty(SenderEmailAddress))
        {
            throw new Exception($"Need to configure {ConfigSection}.{nameof(SenderEmailAddress)} in appsettings");
        }
        if (string.IsNullOrEmpty(SmtpFolder))
        {
            throw new Exception($"Need to configure {ConfigSection}.{nameof(SmtpFolder)} in appsettings");
        }
    }
}