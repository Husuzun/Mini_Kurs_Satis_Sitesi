namespace Mini_Kurs_Satis_Sitesi.Core.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CourseId { get; set; }
        public decimal Price { get; set; }
        
        // Navigation properties
        public Order Order { get; set; }
        public Course Course { get; set; }
    }
} 