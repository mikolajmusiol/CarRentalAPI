using CarRentalAPI.Models;
using CarRentalAPI.Services;
using CarRentalAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
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
            var orderDtos = _orderService.GetAllOrders();
            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderDto> GetOrder([FromRoute] int id)
        {
            var orderDto = _orderService.GetOrderById(id);
            return Ok(orderDto);
        }

        [HttpPost]
        public ActionResult CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            int id = _orderService.CreateOrder(orderDto);
            return Created($"api/orders/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateOrder([FromRoute] int id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            _orderService.UpdateById(id, updateOrderDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteOrder([FromRoute] int id)
        {
            _orderService.DeleteById(id);
            return NoContent();
        }
    }
}
