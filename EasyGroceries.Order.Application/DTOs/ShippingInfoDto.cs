

namespace EasyGroceries.Order.Application.DTOs
{
    public class ShippingInfoDto
    {
        public int UserId { get; set; }
        public double OrderTotal { get; set; }
        public string Name { get; set; }
        public string ApartmentName { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public IEnumerable<OrderDetailsDto> OrderDetails { get; set; }
    }
}