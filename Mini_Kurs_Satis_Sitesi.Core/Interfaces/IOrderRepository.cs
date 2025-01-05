using Mini_Kurs_Satis_Sitesi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Core.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetUserOrders(string userId);
        Task<Order> GetOrderWithDetails(int orderId);
        Task<IEnumerable<Order>> GetOrdersByStatus(string status);
    }
} 