using EasyGroceries.Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Contracts.Infrastructure
{
    public interface IOrderDetailsRepository
    {
        Task<IReadOnlyList<OrderDetails>> GetAllOrderDetails();
        Task<IReadOnlyList<OrderDetails>> GetOrderDetailsByOrderHeaderId(int headerId);
        Task Add(OrderDetails orderDetails);
    }
}
