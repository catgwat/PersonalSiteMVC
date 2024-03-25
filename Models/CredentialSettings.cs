namespace PersonalSiteMVC.Models
{
    public class CredentialSettings
    {
        public Email Email { get; set; } = null!;
    }
    public class Email
    {
        //The property names here MUST match the keys
        public string Server { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Recipient { get; set; } = null!;
    }
}
