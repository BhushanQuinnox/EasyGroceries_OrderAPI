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
    public class OrderHeaderRepository : IOrderHeaderRepository
    {
        private readonly IDapper _dapper;

        public OrderHeaderRepository(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task Add(OrderHeader orderHeader)
        {
            var query = "Insert into OrderHeader (OrderHeaderId, UserId, LoyaltyMembershipOpted, OrderTotal, OrderTime, OrderStatus) values (@OrderHeaderId, @UserId, @LoyaltyMembershipOpted, @OrderTotal, @OrderTime, @OrderStatus)";
            await Task.FromResult(_dapper.Insert<OrderHeader>(query, orderHeader, commandType: CommandType.Text));
        }

        public async Task<OrderHeader> GetOrderHeaderByUserId(int id)
        {
            var query = "SELECT * FROM OrderHeader WHERE UserId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.Int32, ParameterDirection.Input);
            var orderHeader = await Task.FromResult(_dapper.Get<OrderHeader>(query, parameters, CommandType.Text));
            return orderHeader;
        }
    }
}
