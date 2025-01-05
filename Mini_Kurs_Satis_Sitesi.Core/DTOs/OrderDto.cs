using System;
using System.Collections.Generic;

namespace Mini_Kurs_Satis_Sitesi.Core.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public PaymentDto Payment { get; set; }
    }

    public class CreateOrderDto
    {
        public string? UserId { get; set; }
        public List<OrderItemCreateDto> OrderItems { get; set; }
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderItemCreateDto
    {
        public int CourseId { get; set; }
    }
} 