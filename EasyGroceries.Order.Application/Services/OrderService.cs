﻿using AutoMapper;
using EasyGroceries.Order.Application.Contracts.MessageBus;
using EasyGroceries.Order.Application.Contracts.Services;
using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Order.Application.Features.OrderDetails.Requests.Commands;
using EasyGroceries.Order.Application.Features.OrderDetails.Requests.Queries;
using EasyGroceries.Order.Application.Features.OrderHeader.Requests.Commands;
using EasyGroceries.Order.Application.Features.OrderHeader.Requests.Queries;
using EasyGroceries.Order.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ResponseDto<OrderHeaderDto>> GetOrderByUserId(int userId)
        {
            ResponseDto<OrderHeaderDto> response = new ResponseDto<OrderHeaderDto>();
            OrderHeaderDto headerDto = await _mediator.Send(new GetOrderHeaderByUserIdRequest() { UserId = userId });
            headerDto.OrderDetails = await _mediator.Send(new GetOrderDetailsByHeaderIdRequest() { HeaderId = headerDto.OrderHeaderId });
            response.Result = headerDto;
            response.Status = (int)HttpStatusCode.OK;
            return response;
        }

        public async Task<ResponseDto<OrderHeaderDto>> CreateOrder(CartDto cartDto)
        {
            ResponseDto<OrderHeaderDto> response = new ResponseDto<OrderHeaderDto>();
            OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
            orderHeaderDto.OrderStatus = Domain.OrderStatusEnum.Processed;
            orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);
            orderHeaderDto.OrderTotal = CalculateOrderTotal(orderHeaderDto.OrderTotal, orderHeaderDto.LoyaltyMembershipOpted);
            var result = await _mediator.Send(new CreateOrderHeaderRequest() { OrderHeaderDto = orderHeaderDto });

            if (result.IsSuccess)
            {
                var isSuccess = await _mediator.Send(new CreateOrderDetailsListRequest() { OrderDetailsDtoLst = orderHeaderDto.OrderDetails.ToList() });
                if (isSuccess)
                {
                    response.Result = result.Result;
                    response.Status = result.Status;
                    response.IsSuccess = result.IsSuccess;
                    response.Message = result.Message;
                }
            }

            return response;
        }


        private double CalculateOrderTotal(double orderTotal, bool isLoyaltyMembershipOpted)
        {
            var calculatedOrderTotal = orderTotal;
            if (isLoyaltyMembershipOpted)
            {
                var discount = (double)(orderTotal * (20.0 / 100.0));
                calculatedOrderTotal -= discount;
                calculatedOrderTotal += 5;
            }

            return calculatedOrderTotal;
        }
    }
}
