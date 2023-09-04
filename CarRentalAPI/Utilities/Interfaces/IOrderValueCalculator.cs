using CarRentalAPI.Entities;

namespace CarRentalAPI.Utilities.Interfaces
{
    public interface IOrderValueCalculator
    {
        decimal CalculateOrderValue(Order order, Price price);
    }
}