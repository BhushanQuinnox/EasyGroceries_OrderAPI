using AutoMapper;
using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Order.Application.Features.OrderHeader.Requests.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Features.OrderHeader.Handlers.Queries
{
    public class GetOrderHeaderByUserIdRequestHandler : IRequestHandler<GetOrderHeaderByUserIdRequest, OrderHeaderDto>
    {
        private readonly IOrderHeaderRepository _orderHeaderRepository;
        private readonly IMapper _mapper;

        public GetOrderHeaderByUserIdRequestHandler(IOrderHeaderRepository orderHeaderRepository, IMapper mapper)
        {
            _orderHeaderRepository = orderHeaderRepository;
            _mapper = mapper;
        }

        public async Task<OrderHeaderDto> Handle(GetOrderHeaderByUserIdRequest request, CancellationToken cancellationToken)
        {
            var orderHeader = await _orderHeaderRepository.GetOrderHeaderByUserId(request.UserId);
            return _mapper.Map<OrderHeaderDto>(orderHeader);
        }
    }
}
