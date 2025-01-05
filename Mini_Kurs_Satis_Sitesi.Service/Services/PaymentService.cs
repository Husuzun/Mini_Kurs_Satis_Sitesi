using AutoMapper;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using Mini_Kurs_Satis_Sitesi.Core.UnitOfWork;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Service.Services
{
    public class PaymentService : ServiceGeneric<Payment, PaymentDto>, IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderService _orderService;

        public PaymentService(IPaymentRepository paymentRepository, IOrderService orderService, IUnitOfWork unitOfWork, IMapper mapper)
            : base(paymentRepository, unitOfWork, mapper)
        {
            _paymentRepository = paymentRepository;
            _orderService = orderService;
        }

        async Task<Response<PaymentDto>> IPaymentService.AddAsync(CreatePaymentDto createPaymentDto)
        {
            var payment = _mapper.Map<Payment>(createPaymentDto);
            await _paymentRepository.AddAsync(payment);
            await _unitOfWork.CommitAsync();
            return Response<PaymentDto>.Success(_mapper.Map<PaymentDto>(payment), 200);
        }

        public async Task<Response<PaymentDetailDto>> GetPaymentWithDetails(int id)
        {
            var payment = await _paymentRepository.GetPaymentWithDetails(id);
            if (payment == null)
                return Response<PaymentDetailDto>.Fail("Payment not found", 404, true);

            return Response<PaymentDetailDto>.Success(_mapper.Map<PaymentDetailDto>(payment), 200);
        }

        public async Task<Response<IEnumerable<PaymentDto>>> GetPaymentsByUserId(string userId)
        {
            var payments = await _paymentRepository.GetPaymentsByUserId(userId);
            return Response<IEnumerable<PaymentDto>>.Success(_mapper.Map<IEnumerable<PaymentDto>>(payments), 200);
        }

        async Task<Response<NoDataDto>> IPaymentService.UpdateAsync(UpdatePaymentDto updatePaymentDto, int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
                return Response<NoDataDto>.Fail("Payment not found", 404, true);

            _mapper.Map(updatePaymentDto, payment);
            _paymentRepository.Update(payment);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<PaymentDto>> ProcessPayment(CreatePaymentDto createPaymentDto)
        {
            var orderResult = await _orderService.GetOrderWithDetails(createPaymentDto.OrderId);
            if (orderResult.StatusCode == 404)
                return Response<PaymentDto>.Fail("Order not found", 404, true);

            var payment = _mapper.Map<Payment>(createPaymentDto);
            payment.Amount = orderResult.Data.TotalPrice;
            payment.PaymentDate = DateTime.Now;
            payment.Status = "Processing";
            payment.TransactionId = Guid.NewGuid().ToString();

            await _paymentRepository.AddAsync(payment);
            await _unitOfWork.CommitAsync();

            // Ödeme başarılı olduğunda siparişin durumunu güncelle
            await _orderService.UpdateOrderStatus(payment.OrderId, "Paid");

            return Response<PaymentDto>.Success(_mapper.Map<PaymentDto>(payment), 200);
        }

        public async Task<Response<PaymentDto>> GetPaymentByOrderId(int orderId)
        {
            var payment = await _paymentRepository.GetPaymentByOrderId(orderId);
            if (payment == null)
                return Response<PaymentDto>.Fail("Payment not found", 404, true);

            return Response<PaymentDto>.Success(_mapper.Map<PaymentDto>(payment), 200);
        }

        public async Task<Response<NoDataDto>> ValidatePayment(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
                return Response<NoDataDto>.Fail("Payment not found", 404, true);

            payment.Status = "Validated";
            _paymentRepository.Update(payment);
            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(204);
        }
    }
} 