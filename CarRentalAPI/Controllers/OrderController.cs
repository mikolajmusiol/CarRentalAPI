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
        public ActionResult<List<OrderDto>> GetAllOrders()
        {
            List<OrderDto> orderDtos = _orderService.GetAllOrders();
            return Ok(orderDtos);
        }

        [HttpGet("{orderId}")]
        public ActionResult<OrderDto> GetOrder([FromRoute] int orderId)
        {
            var orderDto = _orderService.GetOrderById(orderId);
            return Ok(orderDto);
        }

        [HttpPost]
        [Authorize(Policy = "IsAdult")]
        public ActionResult CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            int id = _orderService.CreateOrder(orderDto);
            return Created($"api/orders/{id}", null);
        }

        [HttpPut("{orderId}")]
        public ActionResult UpdateOrder([FromRoute] int orderId, [FromBody] UpdateOrderDto updateOrderDto)
        {
            _orderService.UpdateById(orderId, updateOrderDto);
            return Ok();
        }

        [HttpDelete("{orderId}")]
        public ActionResult DeleteOrder([FromRoute] int orderId)
        {
            _orderService.DeleteById(orderId);
            return NoContent();
        }
    }
}