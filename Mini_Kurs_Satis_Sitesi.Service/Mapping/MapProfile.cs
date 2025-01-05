using AutoMapper;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using System;

namespace Mini_Kurs_Satis_Sitesi.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Course, CourseDto>();
            
            CreateMap<CreateCourseDto, Course>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorId));
            
            CreateMap<UpdateCourseDto, Course>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Order mappings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment));

            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"));

            // OrderItem mappings
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItemCreateDto, OrderItem>();

            // Payment mappings
            CreateMap<Payment, PaymentDto>();
            CreateMap<Payment, PaymentDetailDto>();
            CreateMap<CreatePaymentDto, Payment>()
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Processing"))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }
} 