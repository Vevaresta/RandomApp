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
            BillingAddress billingAddress)
        {
            if (shippingAddress == null || billingAddress == null)
                throw new DomainException("Shipping/Billing address is required");
            
            return new Order()
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,
                PaymentMethod = paymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress
            };
        }


        public void ApplyDiscount(decimal discount)
        {
            if (discount < 0 || discount > Subtotal)
                throw new DomainException("Invalid discount amount.");

            Discount = discount;
        }

    }
}
