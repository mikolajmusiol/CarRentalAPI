using CarRentalAPI.Models;

namespace CarRentalAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrders();
        Task<OrderDto> GetOrderById(int orderId);
        Task<int> CreateOrder(CreateOrderDto orderDto);
        Task UpdateById(int orderId, UpdateOrderDto updateOrderDto);
        Task DeleteById(int orderId);
    }
}