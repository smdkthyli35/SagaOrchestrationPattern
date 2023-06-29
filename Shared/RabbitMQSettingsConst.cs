﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class RabbitMQSettingsConst
    {
        public const string OrderSaga = "order-saga-queue";


        public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";
        public const string StockReservedEventQueueName = "stock-reserved-queue";
        public const string OrderPaymentCompletedQueueName = "order-payment-completed-queue";
        public const string OrderPaymentFailedQueueName = "order-payment-failed-queue";
        public const string StockPaymentFailedEventQueueName = "stock-payment-failed-queue";
        public const string OrderStockNotReservedQueueName = "order-stock-not-reserved-queue";
        public const string PaymentStockReservedRequestQueueName = "payment-stock-reserved-request-queue";
        public const string OrderRequestCompletedEventQueueName = "order-request-completed-queue";
    }
}
