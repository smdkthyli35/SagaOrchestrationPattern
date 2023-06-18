using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Dtos;
using Order.API.Models;
using Shared;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrdersController(AppDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto orderCreate)
        {
            Models.Order newOrder = new()
            {
                BuyerId = orderCreate.BuyerId,
                Status = OrderStatus.Suspend,
                Address = new Address
                {
                    Line = orderCreate.AddressDto.Line,
                    District = orderCreate.AddressDto.District,
                    Province = orderCreate.AddressDto.Province
                },
                CreatedDate = DateTime.Now
            };

            orderCreate.orderItems.ForEach(item =>
            {
                newOrder.Items.Add(new OrderItem()
                {
                    Price = item.Price,
                    Count = item.Count,
                    ProductId = item.ProductId
                });
            });

            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            var orderCreatedEvent = new OrderCreatedEvent()
            {
                BuyerId = orderCreate.BuyerId,
                OrderId = newOrder.Id,
                Payment = new PaymentMessage()
                {
                    CardName = orderCreate.Payment.CardName,
                    CardNumber = orderCreate.Payment.CardNumber,
                    CVV = orderCreate.Payment.CVV,
                    Expiration = orderCreate.Payment.Expiration,
                    TotalPrice = orderCreate.orderItems.Sum(x => x.Price * x.Count),
                }
            };

            orderCreate.orderItems.ForEach(item =>
            {
                orderCreatedEvent.orderItems.Add(new OrderItemMessage()
                {
                    Count = item.Count,
                    ProductId = item.ProductId
                });
            });

            await _publishEndpoint.Publish(orderCreatedEvent);

            return Ok();
        }
    }
}
