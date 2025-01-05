using System;

namespace Mini_Kurs_Satis_Sitesi.Core.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string InstructorId { get; set; }
        public string InstructorName { get; set; }
    }
}
