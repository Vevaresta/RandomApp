using RandomApp.OrderManagement.Domain.Enums;
using RandomApp.OrderManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomApp.OrderManagement.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }

        public int CustomerId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public DateTime LastModified {  get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        private readonly List<OrderItem> _orderItems = new List<OrderItem>();

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public ShippingAddress ShippingAddress { get; private set; }
        public BillingAddress BillingAddress { get; private set; }
        public TotalPrice 

        public PaymentMethod PaymentMethod {  get; private set; }

        public PaymentStatus PaymentStatus { get; private set; }
    }
}
