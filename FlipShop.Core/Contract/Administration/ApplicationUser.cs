using FlipShop.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Contract.Administration
{
    public class ApplicationUser : IdentityUser
    {
        //Auto Implemeted Properties ({ get; set; })
        public string FullName { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
