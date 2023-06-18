namespace Order.API.Dtos
{
    public class OrderCreateDto
    {
        public string BuyerId { get; set; }
        public List<OrderItemDto> orderItems { get; set; }
        public PaymentDto Payment { get; set; }
        public AddressDto AddressDto { get; set; }
    }
}
