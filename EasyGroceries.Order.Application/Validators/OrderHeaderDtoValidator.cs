using EasyGroceries.Order.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Validators
{
    public class OrderHeaderDtoValidator : AbstractValidator<OrderHeaderDto>
    {
        public OrderHeaderDtoValidator()
        {

            RuleFor(x => x.OrderDetails.Count())
                .GreaterThan(0)
                .WithMessage("Order should contrain at least one product");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} should not be negative value");

            RuleForEach(x => x.OrderDetails)
                .SetValidator(new OrderDetailsDtoValidator());
        }
    }
}
