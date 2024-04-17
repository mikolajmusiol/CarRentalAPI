using CarRentalAPI.Entities;
using CarRentalAPI.Models.Dtos;
using CarRentalAPI.Utilities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Models.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator(CarRentalDbContext dbContext)
        {
            RuleFor(x => x.CarId)
                .Custom((value, context) =>
                {
                    var car = dbContext.Cars.FirstOrDefault(x => x.Id == value);
                    if (car == null) 
                    {
                        context.AddFailure("CarId", "Car not found");
                    }
                });


            RuleFor(x => new { x.CarId, x.RentalFrom, x.RentalTo })
                .Custom((value, context) =>
                {
                    if (value.RentalFrom > value.RentalTo)
                    {
                        context.AddFailure("Incorrectly selected rental period");
                    }

                    var order = dbContext.Orders.FirstOrDefault(u => u.CarId == value.CarId);

                    if (order != null)
                    {
                        bool carIsRented = CheckOverlap.OfTwoTimePeriods(value.RentalFrom, value.RentalTo, order.RentalFrom, order.RentalTo);

                        if (carIsRented)
                        {
                            context.AddFailure("CarId", "This car is already rented in this time period");
                        }
                    }
                });
        }
    }
}
