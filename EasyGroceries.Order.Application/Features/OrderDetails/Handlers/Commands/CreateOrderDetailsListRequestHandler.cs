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
    public class CreateOrderDetailsListRequestHandler : IRequestHandler<CreateOrderDetailsListRequest, bool>
    {
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IMapper _mapper;

        public CreateOrderDetailsListRequestHandler(IOrderDetailsRepository orderDetailsRepository, IMapper mapper)
        {
            _orderDetailsRepository = orderDetailsRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateOrderDetailsListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new ResponseDto<List<OrderDetailsDto>>();
                var orderDetailsList = _mapper.Map<List<Domain.OrderDetails>>(request.OrderDetailsDtoLst);
                await _orderDetailsRepository.AddOrderDetailsList(orderDetailsList);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}