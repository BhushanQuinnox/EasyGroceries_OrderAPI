using Azure.Messaging.ServiceBus;
using EasyGroceries.Order.Application.Contracts.MessageBus;
using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Order.Application.Validators;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Services
{
    public class MessageBus : IMessageBus
    {
        private readonly IConfiguration _configuration;

        public MessageBus(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseDto<string>> PublishMessage(object message, string queueName)
        {
            ResponseDto<string> response = new ResponseDto<string>();
            var shippingInfo = message as ShippingInfoDto;
            var validator = new ShippingInfoDtoValidator();
            var validationResult = await validator.ValidateAsync(shippingInfo);
            if (!validationResult.IsValid)
            {
                response.Status = (int)HttpStatusCode.BadRequest;
                response.Result = "Failed to queue message to service bus";
                response.IsSuccess = false;
                return response;
            }

            await using var client = new ServiceBusClient(_configuration.GetConnectionString("ServiceBusConnectionString"));
            ServiceBusSender sender = client.CreateSender(queueName);

            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding
                .UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
            response.Status = (int)HttpStatusCode.OK;
            response.Result = "Message queued to service bus successfully";
            response.IsSuccess = true;
            return response;
        }

    }
}
