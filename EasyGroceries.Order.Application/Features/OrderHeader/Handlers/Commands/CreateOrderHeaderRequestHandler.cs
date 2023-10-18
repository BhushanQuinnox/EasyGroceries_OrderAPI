using AutoMapper;
using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Order.Application.Features.OrderHeader.Requests.Commands;
using EasyGroceries.Order.Application.Validators;
using EasyGroceries.Order.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Features.OrderHeader.Handlers.Commands
{
    public class CreateOrderHeaderRequestHandler : IRequestHandler<CreateOrderHeaderRequest, ResponseDto<OrderHeaderDto>>
    {
        private readonly IOrderHeaderRepository _orderHeaderRepository;
        private readonly IMapper _mapper;

        public CreateOrderHeaderRequestHandler(IOrderHeaderRepository orderHeaderRepository, IMapper mapper)
        {
            _orderHeaderRepository = orderHeaderRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto<OrderHeaderDto>> Handle(CreateOrderHeaderRequest request, CancellationToken cancellationToken)
        {
            var validator = new OrderHeaderDtoValidator();
            var validationResult = await validator.ValidateAsync(request.OrderHeaderDto);
            var response = new ResponseDto<OrderHeaderDto>();

            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Status = (int)HttpStatusCode.BadRequest;
                response.Message = "Validation failed while creating OrderHeader";
                return response;
            }

            var orderHeader = _mapper.Map<Domain.OrderHeader>(request.OrderHeaderDto);
            orderHeader.OrderTime = DateTime.Now;
            await _orderHeaderRepository.Add(orderHeader);
            response.Message = "OrderHeader Creation Successful";
            response.Result = request.OrderHeaderDto;
            return response;
        }
    }
}
