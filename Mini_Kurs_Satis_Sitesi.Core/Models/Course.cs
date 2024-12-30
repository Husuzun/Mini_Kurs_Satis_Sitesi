using System.Collections.Immutable;

namespace Mini_Kurs_Satis_Sitesi.Core.Models
{
    public class Course
    {
        public int Id { get; set; } // Unique identifier for the course
        public required string Name { get; set; } = default!; // Name of the course
        public string? Description { get; set; } // Description of the course
        public decimal Price { get; set; } // Price of the course
        public string Category { get; set; } = default!; // Category of the course
        private List<Course> courses { get; set; }
        public ImmutableList<Course> GetCourses => courses.ToImmutableList();


        //static factory method
        public static Course Create(int id, string name, string description, decimal price, string category)
        {
            return new Course { Id = id, Name = name, Description = description, Price = price, Category = category };
        }
    }
}