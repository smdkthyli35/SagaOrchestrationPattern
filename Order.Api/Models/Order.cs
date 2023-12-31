﻿namespace Order.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? FailMessage { get; set; }

        public Address Address { get; set; }
        public OrderStatus Status { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    public enum OrderStatus
    {
        Suspend,
        Complete,
        Fail
    }
}
