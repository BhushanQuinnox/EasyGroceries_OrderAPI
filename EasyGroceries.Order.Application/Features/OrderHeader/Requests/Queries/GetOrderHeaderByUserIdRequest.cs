using EasyGroceries.Order.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Features.OrderHeader.Requests.Queries
{
    public class GetOrderHeaderByUserIdRequest : IRequest<OrderHeaderDto>
    {
        public int UserId { get; set; }
    }
}
