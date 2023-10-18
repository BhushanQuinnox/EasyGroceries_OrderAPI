using Azure.Messaging.ServiceBus;
using EasyGroceries.Order.Application.Contracts.MessageBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Services
{
    public class MessageBus : IMessageBus
    {
        private readonly IConfiguration _configuration;
        private string connectionString = "Endpoint=sb://easygroceries.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LHp8OWpHT9DrstgNvApGVQrCEed9xacew+ASbIk8VVY=";

        public MessageBus(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublishMessage(object message, string queueName)
        {
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
        }
    }
}
