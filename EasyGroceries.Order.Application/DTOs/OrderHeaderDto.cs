using EasyGroceries.Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.DTOs
{
    public class OrderHeaderDto
    {
        public int OrderHeaderId { get; set; }
        public int UserId { get; set; }
        public bool LoyaltyMembershipOpted { get; set; }
        public double OrderTotal { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public string Name { get; set; }
        public string ApartmentName { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public IEnumerable<OrderDetailsDto> OrderDetails { get; set; }
    }
}
