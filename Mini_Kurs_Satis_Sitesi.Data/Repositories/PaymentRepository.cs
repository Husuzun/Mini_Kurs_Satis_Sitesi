using Microsoft.EntityFrameworkCore;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Data.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Payment> GetPaymentByOrderId(int orderId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserId(string userId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Where(p => p.Order.UserId == userId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatus(string status)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<Payment> GetPaymentWithDetails(int id)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Course)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
} 