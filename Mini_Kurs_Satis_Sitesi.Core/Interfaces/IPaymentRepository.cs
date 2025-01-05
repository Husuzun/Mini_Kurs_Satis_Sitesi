using Mini_Kurs_Satis_Sitesi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Core.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment> GetPaymentByOrderId(int orderId);
        Task<IEnumerable<Payment>> GetPaymentsByUserId(string userId);
        Task<IEnumerable<Payment>> GetPaymentsByStatus(string status);
        Task<Payment> GetPaymentWithDetails(int id);
    }
} 