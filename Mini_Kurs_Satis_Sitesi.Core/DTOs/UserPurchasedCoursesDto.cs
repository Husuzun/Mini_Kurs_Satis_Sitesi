using System;
using System.Collections.Generic;

namespace Mini_Kurs_Satis_Sitesi.Core.DTOs
{
    public class UserPurchasedCoursesDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<PurchasedCourseDto> PurchasedCourses { get; set; }
    }

    public class PurchasedCourseDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Category { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public string OrderStatus { get; set; }
    }
} 