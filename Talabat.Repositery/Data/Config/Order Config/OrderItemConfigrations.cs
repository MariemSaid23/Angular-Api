using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities.Order_Aggregate;

namespace Talabat.Repositery.Data.Config.Order_Config
{
    public class OrderItemConfigrations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.OwnsOne(OrderItem => OrderItem.Product, X => X.WithOwner());
            builder.Property(OrderItem => OrderItem.Price)
                .HasColumnType("decimal(12,2)");

        }
    }
}
