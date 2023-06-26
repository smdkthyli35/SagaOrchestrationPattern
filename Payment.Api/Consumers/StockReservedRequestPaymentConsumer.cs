using MassTransit;
using Shared;
using Shared.Events;
using Shared.Interfaces;

namespace Payment.Api.Consumers
{
    public class StockReservedRequestPaymentConsumer : IConsumer<IStockReservedRequestPayment>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<StockReservedRequestPaymentConsumer> _logger;

        public StockReservedRequestPaymentConsumer(ILogger<StockReservedRequestPaymentConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IStockReservedRequestPayment> context)
        {
            var balance = 3000m;

            if (balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($" {context.Message.Payment.TotalPrice} TL was withdrawn from credit card for Buyer Id: {context.Message.BuyerId}");

                await _publishEndpoint.Publish(new PaymentCompletedEvent(context.Message.CorrelationId));
            }
            else
            {
                _logger.LogInformation($" {context.Message.Payment.TotalPrice} TL was not withdrawn from credit card for user id = {context.Message.BuyerId}");

                await _publishEndpoint.Publish(new PaymentFailedEvent(context.Message.CorrelationId)
                {
                    OrderItems = context.Message.OrderItems,
                    Reason = "Not enough balance!"
                });
            }
        }
    }
}
