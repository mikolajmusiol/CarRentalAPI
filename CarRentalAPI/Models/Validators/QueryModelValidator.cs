using CarRentalAPI.Entities;
using FluentValidation;

namespace CarRentalAPI.Models.Validators
{
    public class QueryModelValidator : AbstractValidator<QueryModel>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };

        private string[] allowedSortByColumnNames =
        {    nameof(Car.Brand), nameof(Car.Model), nameof(Car.Mileage), 
             nameof(Car.Price.PriceForAnHour), nameof(Car.Price.PriceForADay), 
             nameof(Car.Price.PriceForADay), nameof(Car.YearOfProduction),
             nameof(Order.Value), nameof(Order.RentalFrom), nameof(Order.RentalTo)
        };

        public QueryModelValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
                }
            });

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"SortBy is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}
