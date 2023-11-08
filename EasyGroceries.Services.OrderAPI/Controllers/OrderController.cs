﻿using System.Net;
using Azure;
using EasyGroceries.Order.Application.Contracts.MessageBus;
using EasyGroceries.Order.Application.Contracts.Services;
using EasyGroceries.Order.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyGroceries.Services.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderService orderService, IMessageBus messageBus, IConfiguration configuration)
        {
            _orderService = orderService;
            _messageBus = messageBus;
            _configuration = configuration;
        }

        [HttpGet("GetOrders")]
        public async Task<ActionResult<ResponseDto<OrderHeaderDto>>> Get(int userId)
        {
            var response = await _orderService.GetOrderByUserId(userId);
            if (response.Result == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<ResponseDto<OrderHeaderDto>>> CreateOrder([FromBody] CartDto cartDto)
        {
            var response = await _orderService.CreateOrder(cartDto);
            return Ok(response);
        }

        [HttpPost("GenerateShippingSlip")]
        public async Task<ActionResult<ResponseDto<string>>> GenerateShippingSlip([FromBody] ShippingInfoDto shippingInfoDto)
        {
            ResponseDto<string> response = await _messageBus.PublishMessage(shippingInfoDto, _configuration.GetValue<string>("TopicAndQueueNames:GenerateShippingSlipQueue"));
            return response;
        }
    }
}
