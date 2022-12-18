namespace ViThor.Auth.Settings
{
    public class ViThorAuthSettings
    {
        public string BaseAddress { get; set; } = string.Empty;
        public JwtConfig JwtConfig { get; set; } = null!;
        public SmtpConfig SmtpConfig { get; set; } = null!;
    }
}