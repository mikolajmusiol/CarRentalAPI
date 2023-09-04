using CarRentalAPI.Models;
using CarRentalAPI.Services;
using CarRentalAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("api/account/{accountId}/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public ActionResult<List<OrderDto>> GetAllOrders([FromRoute] int accountId)
        {
            List<OrderDto> orderDtos = _orderService.GetAllOrders(accountId);
            return Ok(orderDtos);
        }

        [HttpGet("{orderId}")]
        public ActionResult<OrderDto> GetOrder([FromRoute] int accountId, [FromRoute] int orderId)
        {
            var orderDto = _orderService.GetOrderById(accountId, orderId);
            return Ok(orderDto);
        }

        [HttpPost]
        public ActionResult CreateOrder([FromRoute] int accountId, [FromBody] CreateOrderDto orderDto)
        {
            int id = _orderService.CreateOrder(accountId, orderDto);
            return Created($"api/orders/{id}", null);
        }

        [HttpPut("{orderId}")]
        public ActionResult UpdateOrder([FromRoute] int accountId, [FromRoute] int orderId, [FromBody] UpdateOrderDto updateOrderDto)
        {
            _orderService.UpdateById(accountId, orderId, updateOrderDto);
            return Ok();
        }

        [HttpDelete("{orderId}")]
        public ActionResult DeleteOrder([FromRoute] int accountId, [FromRoute] int orderId)
        {
            _orderService.DeleteById(accountId, orderId);
            return NoContent();
        }
    }
}