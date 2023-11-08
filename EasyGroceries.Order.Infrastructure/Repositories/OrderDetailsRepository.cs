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
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public OrderDetailsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task Add(OrderDetails orderDetails)
        {
            var sqlCommand = "Insert into OrderDetails (OrderDetailsId, OrderHeaderId, ProductId, ProductName, Price, Count) values (@OrderDetailsId, @OrderHeaderId, @ProductId, @ProductName, @Price, @Count)";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sqlCommand, new
                {
                    OrderDetailsId = orderDetails.OrderDetailsId,
                    OrderHeaderId = orderDetails.OrderHeaderId,
                    ProductId = orderDetails.ProductId,
                    ProductName = orderDetails.ProductName,
                    Price = orderDetails.Price,
                    Count = orderDetails.Count,
                });

                connection.Close();
            }
        }

        public async Task AddOrderDetailsList(List<OrderDetails> orderDetailsLst)
        {
            var sqlCommand = "Insert into OrderDetails (OrderDetailsId, OrderHeaderId, ProductId, ProductName, Price, Count) values (@OrderDetailsId, @OrderHeaderId, @ProductId, @ProductName, @Price, @Count)";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sqlCommand, orderDetailsLst);
                connection.Close();
            }
        }

        public async Task<IReadOnlyList<OrderDetails>> GetAllOrderDetails()
        {
            var sqlCommand = "SELECT * FROM OrderDetails";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<OrderDetails>(sqlCommand);
                return result.ToList();
            }
        }

        public async Task<IReadOnlyList<OrderDetails>> GetOrderDetailsByOrderHeaderId(int id)
        {
            var sql = "SELECT * FROM OrderDetails WHERE OrderHeaderId = @id";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<OrderDetails>(sql, new { id });
                return result.ToList();
            }
        }
    }
}
