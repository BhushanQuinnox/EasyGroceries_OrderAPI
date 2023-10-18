using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Infrastructure.Repositories
{
    public class OrderHeaderRepository : IOrderHeaderRepository
    {
        private static List<OrderHeader> _orderHeaderList = new List<OrderHeader>();

        public async Task Add(OrderHeader orderHeader)
        {
            _orderHeaderList.Add(orderHeader);
        }

        public Task Delete(OrderHeader orderHeader)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<OrderHeader>> GetAllOrderHeaders()
        {
            throw new NotImplementedException();
        }

        public async Task<OrderHeader> GetOrderHeaderByUserId(int userId)
        {
            var orderHeader = _orderHeaderList.FirstOrDefault(x => x.UserId == userId);
            return orderHeader;
        }

        public Task Update(int id, OrderHeader orderHeader)
        {
            throw new NotImplementedException();
        }
    }
}
