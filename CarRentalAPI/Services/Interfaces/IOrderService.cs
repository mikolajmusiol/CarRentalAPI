using CarRentalAPI.Models;
using CarRentalAPI.Models.Dtos;

namespace CarRentalAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<PagedResult<OrderDto>> GetAllOrders(QueryModel query);
        Task<OrderDto> GetOrderById(int orderId);
        Task<int> CreateOrder(CreateOrderDto orderDto);
        Task UpdateById(int orderId, UpdateOrderDto updateOrderDto);
        Task DeleteById(int orderId);
    }
}