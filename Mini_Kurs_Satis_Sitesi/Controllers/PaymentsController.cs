using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.Constants;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : CustomBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentsController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(CreatePaymentDto createPaymentDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orderResult = await _orderService.GetOrderWithDetails(createPaymentDto.OrderId);

            if (orderResult.Data?.UserId != userId && !User.IsInRole(Roles.Instructor))
                return Forbid();

            return ActionResultInstance(await _paymentService.ProcessPayment(createPaymentDto));
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentByOrder(int orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orderResult = await _orderService.GetOrderWithDetails(orderId);

            if (orderResult.Data?.UserId != userId)
                return Forbid();

            return ActionResultInstance(await _paymentService.GetPaymentByOrderId(orderId));
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserPayments()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return ActionResultInstance(await _paymentService.GetPaymentsByUserId(userId));
        }

        [Authorize(Roles = Roles.Instructor)]
        [HttpPost("{id}/validate")]
        public async Task<IActionResult> ValidatePayment(int id)
        {
            return ActionResultInstance(await _paymentService.ValidatePayment(id));
        }

        [Authorize(Roles = Roles.Instructor)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPayments()
        {
            return ActionResultInstance(await _paymentService.GetAllAsync());
        }
    }
}