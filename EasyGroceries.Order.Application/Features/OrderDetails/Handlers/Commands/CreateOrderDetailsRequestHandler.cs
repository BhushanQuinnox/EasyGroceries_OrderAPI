using AutoMapper;
using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Order.Application.Features.OrderDetails.Requests.Commands;
using EasyGroceries.Order.Application.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Features.OrderDetails.Handlers.Commands
{
    public class CreateOrderDetailsRequestHandler : IRequestHandler<CreateOrderDetailsRequest, bool>
    {
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IMapper _mapper;

        public CreateOrderDetailsRequestHandler(IOrderDetailsRepository orderDetailsRepository, IMapper mapper)
        {
            _orderDetailsRepository = orderDetailsRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateOrderDetailsRequest request, CancellationToken cancellationToken)
        {
            var validator = new OrderDetailsDtoValidator();
            var validationResult = await validator.ValidateAsync(request.OrderDetailsDto);
            var response = new ResponseDto<OrderDetailsDto>();

            if (!validationResult.IsValid)
            {
                return false;
            }

            var orderDetails = _mapper.Map<Domain.OrderDetails>(request.OrderDetailsDto);
            await _orderDetailsRepository.Add(orderDetails);
            response.Message = "OrderDetails Creation Successful";
            response.Result = request.OrderDetailsDto;
            return response.IsSuccess;
        }
    }
}
