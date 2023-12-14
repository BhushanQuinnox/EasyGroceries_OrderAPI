using Dapper;
using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Domain;
using EasyGroceries.Order.Infrastructure.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Infrastructure.Repositories
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly IDapper _dapper;

        public OrderDetailsRepository(IDapper dapper)
        {
            _dapper = dapper;
        }


        public async Task Add(OrderDetails orderDetails)
        {
            var query = "Insert into OrderDetails (OrderDetailsId, OrderHeaderId, ProductId, ProductName, Price, Count) values (@OrderDetailsId, @OrderHeaderId, @ProductId, @ProductName, @Price, @Count)";
            await Task.FromResult(_dapper.Insert<OrderDetails>(query, orderDetails, commandType: CommandType.Text));
        }

        public async Task AddOrderDetailsList(List<OrderDetails> orderDetailsLst)
        {
            var query = "Insert into OrderDetails (OrderDetailsId, OrderHeaderId, ProductId, ProductName, Price, Count) values (@OrderDetailsId, @OrderHeaderId, @ProductId, @ProductName, @Price, @Count)";
            await Task.FromResult(_dapper.InsertList<List<OrderDetails>>(query, orderDetailsLst, commandType: CommandType.Text));
        }

        public async Task<IReadOnlyList<OrderDetails>> GetAllOrderDetails()
        {
            var query = "SELECT * FROM OrderDetails";
            var orderDetailsList = await Task.FromResult(_dapper.GetAll<OrderDetails>(query, null, commandType: CommandType.Text));
            return orderDetailsList;
        }

        public async Task<IReadOnlyList<OrderDetails>> GetOrderDetailsByOrderHeaderId(int id)
        {
            var query = "SELECT * FROM OrderDetails";
            var allOrderDetails = await Task.FromResult(_dapper.GetAll<OrderDetails>(query, null, commandType: CommandType.Text));
            var orderDetailsById = allOrderDetails.FindAll(x => x.OrderHeaderId == id);
            return orderDetailsById;
        }
    }
}
