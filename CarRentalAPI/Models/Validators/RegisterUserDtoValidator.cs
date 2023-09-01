﻿using CarRentalAPI.Entities;
using FluentValidation;

namespace CarRentalAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(CarRentalDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(comparer => comparer.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    bool emailInUse = dbContext.Users.Any(u => u.Email == value);

                    if (emailInUse)
                    {
                        context.AddFailure("Email", "Email is taken");
                    }
                });

            RuleFor(x => x.PostalCode)
                .MaximumLength(6)
                .Matches(@"\d\d-\d{3}");
        }
    }
}
