using CarRentalAPI.Entities;
using FluentValidation;

namespace CarRentalAPI.Models.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator(CarRentalDbContext dbContext)
        {
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
                        bool carIsRented = DateInRange(value.RentalFrom, order.RentalFrom, order.RentalTo) || DateInRange(value.RentalTo, order.RentalFrom, order.RentalTo);

                        if (carIsRented)
                        {
                            context.AddFailure("CarId", "This car is already rented in this time period");
                        }
                    }
                });
        }

        private bool DateInRange(DateTime dateTimeToCheck, DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            return dateTimeToCheck >= dateTimeFrom && dateTimeToCheck <= dateTimeTo;
        }
    }
}
