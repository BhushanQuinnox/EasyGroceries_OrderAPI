using Dapper;
using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Infrastructure.Repositories
{
    public class OrderHeaderRepository : IOrderHeaderRepository
    {
        private readonly IConfiguration _configuration;
        public OrderHeaderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Add(OrderHeader orderHeader)
        {
            var sql = "Insert into OrderHeader (OrderHeaderId, UserId, LoyaltyMembershipOpted, OrderTotal, OrderTime, OrderStatus) values (@OrderHeaderId, @UserId, @LoyaltyMembershipOpted, @OrderTotal, @OrderTime, @OrderStatus)";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new
                {
                    OrderHeaderId = orderHeader.OrderHeaderId,
                    UserId = orderHeader.UserId,
                    LoyaltyMembershipOpted = orderHeader.LoyaltyMembershipOpted,
                    OrderTotal = orderHeader.OrderTotal,
                    OrderTime = orderHeader.OrderTime,
                    OrderStatus = (int)orderHeader.OrderStatus
                });

                connection.Close();
            }

        }

        public async Task<OrderHeader> GetOrderHeaderByUserId(int id)
        {
            var sql = "SELECT * FROM OrderHeader WHERE UserId = @id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<OrderHeader>(sql, new { id });
                return result;
            }
        }
    }
}
