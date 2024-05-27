namespace Banana.Services.AuthAPI.Models
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = String.Empty;
        public string Secret { get; set; } = String.Empty;
    }
}
