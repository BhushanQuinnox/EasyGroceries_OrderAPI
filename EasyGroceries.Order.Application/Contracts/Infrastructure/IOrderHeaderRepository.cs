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
        Task<IReadOnlyList<OrderHeader>> GetAllOrderHeaders();
        Task<OrderHeader> GetOrderHeaderByUserId(int userId);
        Task Add(OrderHeader orderHeader);
        Task Update(int id, OrderHeader orderHeader);
        Task Delete(OrderHeader orderHeader);
    }
}
