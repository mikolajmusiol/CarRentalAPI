using CarRentalAPI.Models;

namespace CarRentalAPI.Services.Interfaces
{
    public interface IOrderService
    {
        List<OrderDto> GetAllOrders(int accountId);
        OrderDto GetOrderById(int accountId, int orderId);
        int CreateOrder(int accountId, CreateOrderDto orderDto);
        void UpdateById(int accountId, int orderId, UpdateOrderDto updateOrderDto);
        void DeleteById(int accountId, int orderId);
    }
}
