using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Domain
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }

        public int UserId { get; set; }

        public bool LoyaltyMembershipOpted { get; set; }

        public double OrderTotal { get; set; }
        public DateTime OrderTime { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
