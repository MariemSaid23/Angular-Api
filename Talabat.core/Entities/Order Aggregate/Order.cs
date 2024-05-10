using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        private Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingaddress,int? deliveryMethodId, ICollection<OrderItem> items, decimal subtotal)
        {
            BuyerEmail = buyerEmail;
            Shippingaddress = shippingaddress;
            DeliveryMethodId = deliveryMethodId;
            Items = items;
            Subtotal = subtotal;
        }

        public string BuyerEmail { get; set; } = null;

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Address Shippingaddress { get; set; } = null;

        public int? DeliveryMethodId { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; } = null;

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal Subtotal { get; set; }
       // [NotMapped]
        public decimal GetTotal() => Subtotal +  DeliveryMethod.Cost;
        public string PaymentId { get; set; } = string.Empty;
    }
}
