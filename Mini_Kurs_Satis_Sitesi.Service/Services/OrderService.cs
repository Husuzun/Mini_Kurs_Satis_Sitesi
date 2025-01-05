using AutoMapper;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using Mini_Kurs_Satis_Sitesi.Core.UnitOfWork;
using SharedLibrary.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Mini_Kurs_Satis_Sitesi.Service.Services
{
    public class OrderService : ServiceGeneric<Order, OrderDto>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICourseService _courseService;

        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper, ICourseService courseService)
            : base(orderRepository, unitOfWork, mapper)
        {
            _orderRepository = orderRepository;
            _courseService = courseService;
        }

        async Task<Response<OrderDto>> IOrderService.AddAsync(CreateOrderDto createOrderDto)
        {
            var order = _mapper.Map<Order>(createOrderDto);
            await _orderRepository.AddAsync(order);
            await _unitOfWork.CommitAsync();
            return Response<OrderDto>.Success(_mapper.Map<OrderDto>(order), 200);
        }

        public async Task<Response<OrderDto>> GetOrderWithDetails(int id)
        {
            var order = await _orderRepository.GetOrderWithDetails(id);
            if (order == null)
                return Response<OrderDto>.Fail("Order not found", 404, true);

            return Response<OrderDto>.Success(_mapper.Map<OrderDto>(order), 200);
        }

        public async Task<Response<IEnumerable<OrderDto>>> GetUserOrders(string userId)
        {
            var orders = await _orderRepository.GetUserOrders(userId);
            var orderDtos = orders.Select(order => {
                var dto = _mapper.Map<OrderDto>(order);
                // Her bir order item'ın fiyatını ekle
                dto.OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    CourseId = item.CourseId,
                    CourseName = item.Course.Name,
                    Price = item.Price
                }).ToList();
                return dto;
            });
            return Response<IEnumerable<OrderDto>>.Success(orderDtos, 200);
        }

        async Task<Response<NoDataDto>> IOrderService.UpdateAsync(UpdateOrderDto updateOrderDto, int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return Response<NoDataDto>.Fail("Order not found", 404, true);

            _mapper.Map(updateOrderDto, order);
            _orderRepository.Update(order);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        async Task<Response<OrderDto>> IOrderService.CreateOrder(CreateOrderDto createOrderDto)
        {
            // Kullanıcının daha önce aldığı kursları kontrol et
            var userOrders = await _orderRepository.GetUserOrders(createOrderDto.UserId);
            var purchasedCourseIds = userOrders
                .Where(o => o.Status == "Paid")
                .SelectMany(o => o.OrderItems.Select(oi => oi.CourseId))
                .ToList();

            // Siparişteki kursları kontrol et
            decimal totalPrice = 0;
            var orderItems = new List<OrderItem>();
            
            foreach (var orderItem in createOrderDto.OrderItems)
            {
                if (purchasedCourseIds.Contains(orderItem.CourseId))
                {
                    return Response<OrderDto>.Fail($"Course with ID {orderItem.CourseId} is already purchased", 400, true);
                }

                // Kurs bilgilerini al
                var courseResult = await _courseService.GetByIdAsync(orderItem.CourseId);
                if (courseResult.StatusCode == 404)
                {
                    return Response<OrderDto>.Fail($"Course with ID {orderItem.CourseId} not found", 404, true);
                }

                var course = courseResult.Data;
                orderItems.Add(new OrderItem 
                { 
                    CourseId = orderItem.CourseId,
                    Price = course.Price
                });
                totalPrice += course.Price;
            }

            var order = new Order
            {
                UserId = createOrderDto.UserId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };
            
            await _orderRepository.AddAsync(order);
            await _unitOfWork.CommitAsync();
            
            return Response<OrderDto>.Success(_mapper.Map<OrderDto>(order), 200);
        }

        public async Task<Response<NoDataDto>> UpdateOrderStatus(int id, string status)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return Response<NoDataDto>.Fail("Order not found", 404, true);

            order.Status = status;
            _orderRepository.Update(order);
            await _unitOfWork.CommitAsync();
            
            return Response<NoDataDto>.Success(204);
        }
    }
} 