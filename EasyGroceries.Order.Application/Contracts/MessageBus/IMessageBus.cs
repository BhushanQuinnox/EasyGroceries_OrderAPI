﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Contracts.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage(object message, string queueName);
    }
}
