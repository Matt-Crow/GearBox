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
    }
}