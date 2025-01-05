using System;
using System.Collections.Generic;

namespace Mini_Kurs_Satis_Sitesi.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }  // Pending, Completed, Cancelled etc.
        
        // Navigation properties
        public UserApp User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public Payment Payment { get; set; }
    }
} 