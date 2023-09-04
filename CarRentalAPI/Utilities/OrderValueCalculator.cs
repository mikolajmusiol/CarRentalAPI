using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Utilities.Interfaces;

namespace CarRentalAPI.Utilities
{
    public class OrderValueCalculator : IOrderValueCalculator
    {
        public decimal CalculateOrderValue(Order order, Price price)
        {
            double totalHours = (order.RentalTo - order.RentalFrom).TotalHours;
            double totalDays = (order.RentalTo - order.RentalFrom).TotalDays;

            if (totalHours < 12 && price.PriceForAnHour != null)
            {
                return (decimal)totalHours * price.PriceForAnHour.Value;
            }
            else if (totalDays < 7 && price.PriceForADay != null)
            {
                return (decimal)totalDays * price.PriceForADay.Value;
            }
            else
            {
                return (price.PriceForAWeek != null) ? price.PriceForAWeek.Value : throw new NotFoundException("Price for this time period could not be found");
            }
        }
    }
}