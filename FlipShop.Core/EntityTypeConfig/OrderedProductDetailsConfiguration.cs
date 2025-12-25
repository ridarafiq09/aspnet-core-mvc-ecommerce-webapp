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
    internal class OrderedProductDetailsConfiguration : IEntityTypeConfiguration<OrderedProductDetails>
    {
        public void Configure(EntityTypeBuilder<OrderedProductDetails> builder)
        {
            var OrderedProductDetailsEntity = builder;
            OrderedProductDetailsEntity.ToTable("tbl_ordered_products_details");
            OrderedProductDetailsEntity.HasKey(prop => new { prop.ProductID, prop.Order_ID });
            OrderedProductDetailsEntity.Property(prop => prop.Quantity).IsRequired().HasColumnType("int");
            OrderedProductDetailsEntity.Property(prop => prop.Unit_Price).IsRequired().HasColumnType("money");
            OrderedProductDetailsEntity.HasOne(prop => prop.Product)
            .WithMany(property => property.OrderDetails).HasForeignKey(prop => prop.ProductID);
            OrderedProductDetailsEntity.HasOne(prop => prop.Order)
            .WithMany(property => property.OrderDetails).HasForeignKey(prop => prop.Order_ID);

        }
    }
}
