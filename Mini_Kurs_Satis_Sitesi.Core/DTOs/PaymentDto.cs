using System;

namespace Mini_Kurs_Satis_Sitesi.Core.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
    }

    public class CreatePaymentDto
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string CVV { get; set; }
        public string CardHolderName { get; set; }
    }
} 