using System;

namespace Mini_Kurs_Satis_Sitesi.Core.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }  // Credit Card, PayPal etc.
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }  // Success, Failed, Pending
        
        // Navigation property
        public Order Order { get; set; }
    }
} 