using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Infrastructure.Repositories
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private static List<OrderDetails> _orderDetailsList = new List<OrderDetails>();

        public async Task Add(OrderDetails orderDetails)
        {
            _orderDetailsList.Add(orderDetails);
        }

        public async Task<IReadOnlyList<OrderDetails>> GetAllOrderDetails()
        {
            return _orderDetailsList.ToList();
        }

        public async Task<IReadOnlyList<OrderDetails>> GetOrderDetailsByOrderHeaderId(int headerId)
        {
            var orderDetailsByHeaderId = _orderDetailsList.Where(x => x.OrderHeaderId == headerId).ToList();
            return orderDetailsByHeaderId;
        }
    }
}
