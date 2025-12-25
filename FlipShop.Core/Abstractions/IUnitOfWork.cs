using FlipShop.Core.Abstractions.Accounts;
using FlipShop.Core.Contract.Administration;
using FlipShop.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Account(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager);
        IRepository<Product> ProductRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        Task<int> SaveChanges();
    }
}
