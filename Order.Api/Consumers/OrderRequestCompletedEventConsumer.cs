using MassTransit;
using Order.API.Models;
using Shared.Events;
using Shared.Interfaces;

namespace Order.Api.Consumers
{
    public class OrderRequestCompletedEventConsumer : IConsumer<IOrderRequestCompletedEvent>
    {
        private readonly ILogger<IOrderRequestCompletedEvent> _logger;
        private readonly AppDbContext _context;

        public OrderRequestCompletedEventConsumer(ILogger<IOrderRequestCompletedEvent> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Consume(ConsumeContext<IOrderRequestCompletedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);

            if (order is not null)
            {
                order.Status = OrderStatus.Complete;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Order (Id={context.Message.OrderId}) status has been changed: {order.Status}");
            }
            else
            {
                _logger.LogError($"Order (Id={context.Message.OrderId}) not found.");
            }
        }
    }
}
