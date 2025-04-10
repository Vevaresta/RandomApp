using Common.Shared.Exceptions;
using RandomApp.OrderManagement.Domain.Enums;
using RandomApp.OrderManagement.Domain.ValueObjects;
using System.Reflection.Metadata.Ecma335;

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

        public decimal Subtotal => _orderItems.Sum(item => item.LineTotal);

        public decimal Discount {  get; private set; }
        public decimal Total => Subtotal - Discount;

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public ShippingAddress ShippingAddress { get; private set; }
        public BillingAddress BillingAddress { get; private set; }
        public PaymentMethod PaymentMethod {  get; private set; }

        public PaymentStatus PaymentStatus { get; private set; }

        private Order()
        {

        }

        public static Order Create(
            int customerId,
            PaymentMethod paymentMethod,
            ShippingAddress shippingAddress,
            BillingAddress billingAddress,
            List<OrderItem>? items = null)
        {
            if (shippingAddress == null || billingAddress == null)
                throw new DomainException("Shipping/Billing address is required");

            return new Order()
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId > 0 ? customerId : throw new DomainException("Customer ID must be positive."),
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,
                PaymentMethod = paymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,              
            };
        }

        public void Confirm()
        {
            OrderStatus = OrderStatus switch
            {
                OrderStatus.Pending => OrderStatus.Confirmed,
                OrderStatus.Cancelled => throw new DomainException("Can't confirm a cancelled order."),
                _ => throw new DomainException("Order is already confirmed.")
            };
            LastModified = DateTime.UtcNow;
        }

        public void Cancel()
        {
            OrderStatus = OrderStatus switch
            {
                OrderStatus.Pending or OrderStatus.Confirmed => OrderStatus.Cancelled,
                OrderStatus.Shipped => throw new DomainException("Can't cancel a shipped order."),
                _ => throw new DomainException("Order is already cancelled/delivered.")
            };
            LastModified = DateTime.UtcNow;
        }

        public void Ship()
        {
            if (PaymentStatus != PaymentStatus.Paid)
                throw new DomainException("Order must be paid before shipping.");

            OrderStatus = OrderStatus.Shipped;
            LastModified = DateTime.UtcNow;
        }

        public void Deliver()
        {
            if (OrderStatus != OrderStatus.Shipped)
                throw new DomainException("Order must be shipped before delivery.");

            OrderStatus = OrderStatus.Delivered;
            LastModified = DateTime.UtcNow;
        }


        public void ApplyDiscount(decimal discount)
        {
            if (discount < 0 || discount > Subtotal)
                throw new DomainException("Invalid discount amount.");

            Discount = discount;
        }

        public void RemoveDiscount()
        {
            Discount = 0;
            LastModified = DateTime.UtcNow;
        }

        public void AddItem(OrderItem item)
        {
            _orderItems.Add(item ?? throw new DomainException("Order item can't be null."));
            LastModified = DateTime.UtcNow;
        }

        public void ClearItems()
        {
            if (OrderStatus != OrderStatus.Pending)
                throw new DomainException("Can't clear items after order confirmation");

            _orderItems.Clear();
            LastModified = DateTime.UtcNow;
        }

        public void RemoveItem(int productId)
        {
            if (OrderStatus != OrderStatus.Pending)
                throw new DomainException("Can't modify order after confirmations.");

            var itemToRemove = _orderItems.FirstOrDefault(item => item.ProductId == productId)
                ?? throw new DomainException($"Product {productId} not found in order.");

            _orderItems.Remove(itemToRemove);
            LastModified = DateTime.UtcNow;
        }


    }
}
