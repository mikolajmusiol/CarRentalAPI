using CarRentalAPI.Models;

namespace CarRentalAPI.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderDto> GetAllOrders();
        int CreateOrder(CreateOrderDto orderDto);
        OrderDto GetOrderById(int id);
        void UpdateById(int id, UpdateOrderDto updateOrderDto);
        void DeleteById(int id);
    }
}
