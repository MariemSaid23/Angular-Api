using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackExchange.Redis;
using System.Net.NetworkInformation;

namespace Talabat.core.Entities.Order_Aggregate
{
    internal class OrderConfigrations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.OwnsOne(order => order.Shippingaddress, Shippingaddress => Shippingaddress.WithOwner());
            builder.Property(order => order.Status)
                .HasConversion(
                (OStatus) => OStatus.ToString(),
                 (OStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus));
            builder.Property(order => order.Subtotal)
                .HasColumnType("decimal(12,2)");

        }
    }
}
