namespace Mini_Kurs_Satis_Sitesi.Core.DTOs
{
    public class ClientTokenDto
    {
        public string AccessToken { get; set; } = null!;
        public DateTime AccessTokenExpiration { get; set; }
    }
} 