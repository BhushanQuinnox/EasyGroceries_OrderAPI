using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
