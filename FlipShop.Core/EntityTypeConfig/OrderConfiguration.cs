using FlipShop.Core.Contract.Administration;
using FlipShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.EntityTypeConfig
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            var orderEntity = builder;
            orderEntity.ToTable("tbl_orders");
            orderEntity.HasKey(prop => prop.OrderID);
            orderEntity.Property(prop => prop.OrderStatus).HasColumnType("tinyint").HasDefaultValue(0);
            orderEntity.Property(prop => prop.Payment_Type).IsRequired().HasColumnType("tinyint");
            orderEntity.Property(prop => prop.TotalAmount).IsRequired().HasColumnType("money");
            orderEntity.Property(prop => prop.Order_Date).IsRequired().HasColumnType("datetime2(7)");
            orderEntity.Property(prop => prop.Due_Date).IsRequired().HasColumnType("datetime2(7)");
            orderEntity.Property(prop => prop.SubTotal).IsRequired().HasColumnType("decimal(8,4)");
            orderEntity.Property(prop => prop.DiscountByCoupon).IsRequired().HasColumnType("real");
            orderEntity.Property(prop => prop.ShippingFee).IsRequired().HasColumnType("decimal(8,4)");
            orderEntity.Property(prop => prop.Tax).IsRequired().HasColumnType("decimal(8,4)");
            orderEntity.Property(prop => prop.CustomerID).IsRequired().HasColumnType("nvarchar(450)");
            orderEntity.Property(prop => prop.orderAddressId).IsRequired().HasColumnType("int");

            orderEntity.HasOne<ApplicationUser>(prop => prop.Customer)
                .WithMany(property => property.Orders)
                .HasForeignKey(prop => prop.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);

            orderEntity.HasOne(prop => prop.OrderAddress)
              .WithOne(property => property.Order)
              .HasForeignKey<Order>(prop => prop.orderAddressId)
              .HasConstraintName("FK_Order_OrderAddress");

            orderEntity.HasOne(prop => prop.UserTransactionDetails)
             .WithOne(property => property.Order)
             .HasForeignKey<Order>(prop => prop.userTransactionId);
        }

    }
}
