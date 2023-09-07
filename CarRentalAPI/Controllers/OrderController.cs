using CarRentalAPI.Models;
using CarRentalAPI.Services;
using CarRentalAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetAllOrders()
        {
            List<OrderDto> orderDtos = await _orderService.GetAllOrders();
            return Ok(orderDtos);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDto>> GetOrder([FromRoute] int orderId)
        {
            var orderDto = await _orderService.GetOrderById(orderId);
            return Ok(orderDto);
        }

        [HttpPost]
        [Authorize(Policy = "IsAdult")]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            int id = await _orderService.CreateOrder(orderDto);
            return Created($"api/orders/{id}", null);
        }

        [HttpPut("{orderId}")]
        public async Task<ActionResult> UpdateOrder([FromRoute] int orderId, [FromBody] UpdateOrderDto updateOrderDto)
        {
            await _orderService.UpdateById(orderId, updateOrderDto);
            return Ok();
        }

        [HttpDelete("{orderId}")]
        public async Task<ActionResult> DeleteOrder([FromRoute] int orderId)
        {
            await _orderService.DeleteById(orderId);
            return NoContent();
        }
    }
}