using AutoMapper;
using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Order.Application.Features.OrderDetails.Requests.Queries;
using EasyGroceries.Order.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Features.OrderDetails.Handlers.Queries
{
    public class GetOrderDetailsByHeaderIdRequestHandler : IRequestHandler<GetOrderDetailsByHeaderIdRequest, List<OrderDetailsDto>>
    {
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IMapper _mapper;

        public GetOrderDetailsByHeaderIdRequestHandler(IOrderDetailsRepository orderDetailsRepository, IMapper mapper)
        {
            _orderDetailsRepository = orderDetailsRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDetailsDto>> Handle(GetOrderDetailsByHeaderIdRequest request, CancellationToken cancellationToken)
        {
            var orderDetailsByHeaderId = await _orderDetailsRepository.GetOrderDetailsByOrderHeaderId(request.HeaderId);
            return _mapper.Map<List<OrderDetailsDto>>(orderDetailsByHeaderId);
        }
    }
}
