using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using SharedLibrary.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Core.Interfaces
{
    public interface IOrderService : IServiceGeneric<Order, OrderDto>
    {
        Task<Response<IEnumerable<OrderDto>>> GetUserOrders(string userId);
        Task<Response<OrderDto>> GetOrderWithDetails(int id);
        new Task<Response<OrderDto>> AddAsync(CreateOrderDto createOrderDto);
        new Task<Response<NoDataDto>> UpdateAsync(UpdateOrderDto updateOrderDto, int id);
        Task<Response<OrderDto>> CreateOrder(CreateOrderDto createOrderDto);
        Task<Response<NoDataDto>> UpdateOrderStatus(int id, string status);
    }
} 