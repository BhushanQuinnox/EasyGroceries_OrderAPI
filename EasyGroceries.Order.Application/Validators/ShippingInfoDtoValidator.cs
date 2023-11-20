using EasyGroceries.Order.Application.DTOs;
using FluentValidation;

namespace EasyGroceries.Order.Application.Validators
{
    public class ShippingInfoDtoValidator : AbstractValidator<ShippingInfoDto>
    {
        public ShippingInfoDtoValidator()
        {
            RuleFor(x => x.CustomerInfo.Name)
                .NotNull();

            RuleFor(x => x.UserId)
                .NotNull();

            RuleFor(x => x.OrderTotal)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.CustomerInfo.StreetName)
                .NotNull()
                .Must(y => y.ToLower().Contains("street"))
                .WithMessage("Streetname should contain street word");

            RuleFor(x => x.CustomerInfo.ApartmentName)
                .NotNull();

            RuleFor(x => x.CustomerInfo.City)
                .NotNull();

            RuleFor(x => x.CustomerInfo.Pincode)
                .NotNull();

            RuleFor(x => x.ProductDetails)
                .NotNull()
                .NotEmpty();
        }
    }
}