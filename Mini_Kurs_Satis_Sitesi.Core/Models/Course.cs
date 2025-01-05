using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mini_Kurs_Satis_Sitesi.Core.Models
{
    public class Course
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public string Category { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        
        // Instructor ilişkisi
        public string InstructorId { get; set; }
        public UserApp Instructor { get; set; }
        
        // Navigation properties
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}