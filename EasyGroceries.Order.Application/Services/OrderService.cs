using AutoMapper;
using EasyGroceries.Order.Application.Contracts.Services;
using EasyGroceries.Order.Application.DTOs;
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
            try
            {
                OrderHeaderDto headerDto = await _mediator.Send(new GetOrderHeaderByUserIdRequest() { UserId = userId });
                //headerDto.OrderDetails = await _mediator.Send(new GetOrderDetailsByHeaderIdRequest() { HeaderId = headerDto.OrderHeaderId });
                response.Result = headerDto;
                response.Status = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Status = (int)HttpStatusCode.InternalServerError;
            }

            return response;
        }

        public async Task<ResponseDto<OrderHeaderDto>> CreateOrder(CartDto cartDto)
        {
            ResponseDto<OrderHeaderDto> response = new ResponseDto<OrderHeaderDto>();
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderStatus = Domain.OrderStatusEnum.Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);
                orderHeaderDto.OrderTotal = CalculateOrderTotal(orderHeaderDto.OrderTotal, orderHeaderDto.LoyaltyMembershipOpted);
                await _mediator.Send(new CreateOrderHeaderRequest() { OrderHeaderDto = orderHeaderDto });
                response.Result = orderHeaderDto;
                response.Status = (int)HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
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
