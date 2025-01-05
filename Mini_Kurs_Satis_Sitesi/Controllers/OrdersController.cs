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
    public class OrdersController : CustomBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return ActionResultInstance(await _orderService.GetUserOrders(userId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orderResult = await _orderService.GetOrderWithDetails(id);

            if (orderResult.Data?.UserId != userId && !User.IsInRole(Roles.Instructor))
                return Forbid();

            return ActionResultInstance(orderResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID could not be retrieved from token");
            }

            createOrderDto.UserId = userId;
            return ActionResultInstance(await _orderService.CreateOrder(createOrderDto));
        }

        [Authorize(Roles = Roles.Instructor)]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            return ActionResultInstance(await _orderService.UpdateOrderStatus(id, status));
        }

        [Authorize(Roles = Roles.Instructor)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            return ActionResultInstance(await _orderService.GetAllAsync());
        }
    }
}