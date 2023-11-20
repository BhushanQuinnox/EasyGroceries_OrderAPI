

namespace EasyGroceries.Order.Application.DTOs
{
    public class ShippingInfoDto
    {
        public int UserId { get; set; }
        public double OrderTotal { get; set; }
        public CustomerDto CustomerInfo { get; set; }
        public IEnumerable<ProductDto> ProductDetails { get; set; }
    }
}