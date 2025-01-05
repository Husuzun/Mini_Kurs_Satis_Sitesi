using System.Collections.Generic;

namespace Mini_Kurs_Satis_Sitesi.Core.DTOs
{
    public class InstructorCoursesDto
    {
        public string InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public ICollection<CourseDto> Courses { get; set; }
    }
} 