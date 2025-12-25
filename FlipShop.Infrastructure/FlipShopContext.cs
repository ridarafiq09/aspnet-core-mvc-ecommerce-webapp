using FlipShop.Core.Contract.Administration;
using FlipShop.Core.Entities;
using FlipShop.Core.EntityTypeConfig;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Infrastructure
{
    public class FlipShopContext : IdentityDbContext<ApplicationUser>
    {
        public FlipShopContext(DbContextOptions<FlipShopContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderAddress> OrderAddress { get; set; }
        public DbSet<OrderedProductDetails> OrderedProductDetails { get; set; }
        public DbSet<UserTransactionDetails> UserTransactionDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(OrderConfiguration).Assembly);

            base.OnModelCreating(builder);
        }
    }
}
