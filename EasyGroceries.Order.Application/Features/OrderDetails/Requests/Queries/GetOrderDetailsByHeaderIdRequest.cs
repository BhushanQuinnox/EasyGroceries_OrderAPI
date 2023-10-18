using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Order.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Features.OrderDetails.Requests.Queries
{
    public class GetOrderDetailsByHeaderIdRequest : IRequest<List<OrderDetailsDto>>
    {
        public int HeaderId { get; set; }
    }
}
