namespace SharedLibrary.Configuration
{
    public class CustomTokenOption
    {
        public List<String> Audience { get; set; } = new List<string>();
        public string Issuer { get; set; } = string.Empty;
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string SecurityKey { get; set; } = string.Empty;
    }
} 