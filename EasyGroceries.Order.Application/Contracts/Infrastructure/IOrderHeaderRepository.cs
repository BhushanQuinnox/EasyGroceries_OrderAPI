using EasyGroceries.Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Contracts.Infrastructure
{
    public interface IOrderHeaderRepository
    {
        Task<OrderHeader> GetOrderHeaderByUserId(int id);
        Task Add(OrderHeader orderHeader);
    }
}
