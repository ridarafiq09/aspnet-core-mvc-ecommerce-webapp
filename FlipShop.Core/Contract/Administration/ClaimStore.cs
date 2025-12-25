using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Contract.Administration
{
    public class ClaimStore : UserClaimsPrincipalFactory<ApplicationUser,IdentityRole>
    {
        public ClaimStore(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {


        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UserFirstName", user.FullName ?? ""));
            identity.AddClaim(new Claim("CreateRole", "CreateRole"));
            identity.AddClaim(new Claim("EditRole", "Edit Role"));
            identity.AddClaim(new Claim("DeleteRole", "Delete Role"));
            identity.AddClaim(new Claim("CreateProduct", "CreateProduct"));
            identity.AddClaim(new Claim("EditProduct", "Edit Product"));
            identity.AddClaim(new Claim("DeleteProduct", "Delete Product"));
            return identity;
        }
       
    }
}
