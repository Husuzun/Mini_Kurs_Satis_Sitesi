namespace Mini_Kurs_Satis_Sitesi.Core.DTOs
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }
} 