using System;

namespace Mini_Kurs_Satis_Sitesi.Core.DTOs
{
    public class PaymentDetailDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public OrderDto Order { get; set; }
    }
} 