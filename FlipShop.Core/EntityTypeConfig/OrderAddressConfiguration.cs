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
    internal class OrderAddressConfiguration : IEntityTypeConfiguration<OrderAddress>
    {
       
        public void Configure(EntityTypeBuilder<OrderAddress> builder)
        {
            var orderAddressEntity = builder;
            orderAddressEntity.ToTable("tbl_order_address");
            orderAddressEntity.HasKey(prop => prop.AddressId);
            orderAddressEntity.Property(prop => prop.AddressLine1).HasColumnType("varchar(500)");
            orderAddressEntity.Property(prop => prop.AddressLine2).HasColumnType("varchar(500)");
            orderAddressEntity.Property(prop => prop.State).IsRequired().HasColumnType("varchar(100)");
            orderAddressEntity.Property(prop => prop.Postal).IsRequired().HasColumnType("smallint");
            orderAddressEntity.Property(prop => prop.City).IsRequired().HasColumnType("varchar(100)");
            orderAddressEntity.Property(prop => prop.Country).IsRequired().HasColumnType("varchar(100)");

        }
    }
}
