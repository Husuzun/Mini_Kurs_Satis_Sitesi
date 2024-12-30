namespace Mini_Kurs_Satis_Sitesi.Core.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; } // Unique identifier for the course
        public required string Name { get; set; } = default!; // Name of the course
        public string? Description { get; set; } // Description of the course
        public decimal Price { get; set; } // Price of the course
        public string Category { get; set; } = default!; // Category of the course
    }
}
