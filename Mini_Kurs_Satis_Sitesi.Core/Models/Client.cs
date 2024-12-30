namespace Mini_Kurs_Satis_Sitesi.Core.Models
{
    public class Client
    {
        public string Id { get; set; } = null!;
        public string Secret { get; set; } = null!;
        public List<string> Audiences { get; set; } = null!;
    }
} 