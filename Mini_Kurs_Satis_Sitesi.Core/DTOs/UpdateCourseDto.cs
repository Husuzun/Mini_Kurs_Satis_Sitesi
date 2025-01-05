namespace Mini_Kurs_Satis_Sitesi.Core.DTOs
{
    public class UpdateCourseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 