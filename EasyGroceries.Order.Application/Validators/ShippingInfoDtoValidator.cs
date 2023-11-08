using EasyGroceries.Order.Application.DTOs;
using FluentValidation;

namespace EasyGroceries.Order.Application.Validators
{
    public class ShippingInfoDtoValidator : AbstractValidator<ShippingInfoDto>
    {
        public ShippingInfoDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull();

            RuleFor(x => x.UserId)
                .NotNull();

            RuleFor(x => x.OrderTotal)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.StreetName)
                .NotNull()
                .Must(y => y.ToLower().Contains("street"))
                .WithMessage("Streetname should contain street word");

            RuleFor(x => x.ApartmentName)
                .NotNull();

            RuleFor(x => x.City)
                .NotNull();

            RuleFor(x => x.Pincode)
                .NotNull();

            RuleFor(x => x.OrderDetails)
                .NotNull()
                .NotEmpty();
        }
    }
}