﻿using MassTransit;
using Order.API.Models;
using Shared.Interfaces;

namespace Order.Api.Consumers
{
    public class OrderRequestFailedEventConsumer : IConsumer<IOrderRequestFailedEvent>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderRequestFailedEventConsumer> _logger;

        public OrderRequestFailedEventConsumer(AppDbContext context, ILogger<OrderRequestFailedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IOrderRequestFailedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order is not null)
            {
                order.Status = OrderStatus.Fail;
                order.FailMessage = context.Message.Reason;
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
