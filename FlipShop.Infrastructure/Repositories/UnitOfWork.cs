using FlipShop.Core.Abstractions;
using FlipShop.Core.Abstractions.Accounts;
using FlipShop.Core.Contract.Administration;
using FlipShop.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposedValue;
        private FlipShopContext _context;

        public UnitOfWork(FlipShopContext context)
        {
            _context = context;
            //Account = new AccountRepository(_context);
        }

        private IAccountRepository? account;
        public IAccountRepository Account(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            if (account is null)
            {
                account = new AccountRepository(_context, userManager, signInManager);
            }

            return account;
        }
        private IRepository<Category> categoryRepo;
        public IRepository<Category> CategoryRepository
        {
            get
            {
                if (categoryRepo == null)
                {
                    categoryRepo = new Repository<Category>(_context);
                }
                return categoryRepo;
            }
        }
        private IRepository<Product> productRepo;
        public IRepository<Product> ProductRepository
        {
            get
            {

                if (productRepo == null)
                {
                    productRepo = new Repository<Product>(_context);
                }

                return productRepo;
            }
        }
        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _context = null;
              
                disposedValue = true;
            }
        }


        public void Dispose()
        { 
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
