using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using SharedLibrary.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Core.Interfaces
{
    public interface IPaymentService : IServiceGeneric<Payment, PaymentDto>
    {
        Task<Response<IEnumerable<PaymentDto>>> GetPaymentsByUserId(string userId);
        Task<Response<PaymentDetailDto>> GetPaymentWithDetails(int id);
        new Task<Response<PaymentDto>> AddAsync(CreatePaymentDto createPaymentDto);
        new Task<Response<NoDataDto>> UpdateAsync(UpdatePaymentDto updatePaymentDto, int id);
        Task<Response<PaymentDto>> ProcessPayment(CreatePaymentDto createPaymentDto);
        Task<Response<PaymentDto>> GetPaymentByOrderId(int orderId);
        Task<Response<NoDataDto>> ValidatePayment(int id);
    }
} 