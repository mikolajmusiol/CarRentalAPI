using CarRentalAPI.Models;

namespace CarRentalAPI.Services.Interfaces
{
    public interface IOrderService
    {
        List<OrderDto> GetAllOrders();
        OrderDto GetOrderById(int orderId);
        int CreateOrder(CreateOrderDto orderDto);
        void UpdateById(int orderId, UpdateOrderDto updateOrderDto);
        void DeleteById(int orderId);
    }
}