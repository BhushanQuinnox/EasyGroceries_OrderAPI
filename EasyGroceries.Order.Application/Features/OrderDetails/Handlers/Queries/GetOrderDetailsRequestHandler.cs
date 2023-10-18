using AutoMapper;
using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Order.Application.Features.OrderDetails.Requests.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Features.OrderDetails.Handlers.Queries
{
    public class GetOrderDetailsRequestHandler : IRequestHandler<GetOrderDetailsRequest, List<OrderDetailsDto>>
    {
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IMapper _mapper;

        public GetOrderDetailsRequestHandler(IOrderDetailsRepository orderDetailsRepository, IMapper mapper)
        {
            _orderDetailsRepository = orderDetailsRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDetailsDto>> Handle(GetOrderDetailsRequest request, CancellationToken cancellationToken)
        {
            var orderDetails = await _orderDetailsRepository.GetAllOrderDetails();
            return _mapper.Map<List<OrderDetailsDto>>(orderDetails);
        }
    }
}
