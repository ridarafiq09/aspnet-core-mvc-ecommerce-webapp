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
    internal class UserTransactionalDetailsConfiguration : IEntityTypeConfiguration<UserTransactionDetails>
    {
        public void Configure(EntityTypeBuilder<UserTransactionDetails> builder)
        {
            var userTransactionEntity = builder;
            userTransactionEntity.ToTable("tbl_user_transaction_details");
            userTransactionEntity.HasKey(prop => prop.TransactionId);
            userTransactionEntity.Property(prop => prop.NameOnCard).IsRequired().HasColumnType("varchar(100)");
            userTransactionEntity.Property(prop => prop.CardNumber).IsRequired().HasColumnType("bigint");
            userTransactionEntity.Property(prop => prop.ExpirationDate).IsRequired().HasColumnType("datetime2(7)");
            userTransactionEntity.Property(prop => prop.CVV).IsRequired().HasColumnType("smallint");
        }
    }
}
