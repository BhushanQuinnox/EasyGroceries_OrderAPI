using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyGroceries.Order.Application.DTOs;

namespace EasyGroceries.Order.Application.Contracts.MessageBus
{
    public interface IMessageBus
    {
        Task<ResponseDto<string>> PublishMessage(object message, string queueName);
    }
}
