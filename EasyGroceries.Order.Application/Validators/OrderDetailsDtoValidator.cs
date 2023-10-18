using EasyGroceries.Order.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Validators
{
    public class OrderDetailsDtoValidator : AbstractValidator<OrderDetailsDto>
    {
        public OrderDetailsDtoValidator()
        {
            RuleFor(x => x.Product)
                .NotNull();

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("{PropertyName} should be greater than 0");

            RuleFor(x => x.ProductName)
                .NotNull();
        }
    }
}
