namespace Vou.Services.AuthAPI.Models
{
    public class JwtOption
    {
        public string Secret { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
    }
}
