using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlipShop.Core.Abstractions;
using FlipShop.Core.Abstractions.Accounts;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using AuthenticationProperties = Microsoft.AspNetCore.Authentication.AuthenticationProperties;
using FlipShop.Core.Contract.Administration;
using Microsoft.IdentityModel.Claims;
using Claim = System.Security.Claims.Claim;
using ClaimsPrincipal = System.Security.Claims.ClaimsPrincipal;

namespace FlipShop.Infrastructure.Repositories
{
    public class AccountRepository : Repository<ApplicationUser>, IAccountRepository
    {
        private  UserManager<ApplicationUser> userManagerService;
        private  SignInManager<ApplicationUser> signInManagerService;


        public AccountRepository(FlipShopContext context, UserManager<ApplicationUser> userManagerService, SignInManager<ApplicationUser> signInManagerService) : base(context)
        {
            this.userManagerService = userManagerService;
            this.signInManagerService = signInManagerService;
        }

        public async Task<IdentityResult> AddUser(Account user)
        {
            ApplicationUser applicationUser = new()
            {
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.UserName
            };

            IdentityResult identityResult = await userManagerService
                .CreateAsync(applicationUser, user.Password);

            if (identityResult.Succeeded)
            {
                await signInManagerService.SignInAsync(applicationUser, isPersistent: false);
 
            }
            return identityResult;

        }

        public async Task<SignInResult> SignIn(Account user)
        {
            return await signInManagerService
                .PasswordSignInAsync(user.UserName, user.Password, user.RememberMe, true);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user,string token)
        {
            return await userManagerService
                .ConfirmEmailAsync(user,token);
        }

        public async Task<string?> ForgotPassword(string email)
        {
            var user = await userManagerService.FindByEmailAsync(email);
            if(user is not null && await userManagerService.IsEmailConfirmedAsync(user))
            {
                var token = await userManagerService.GeneratePasswordResetTokenAsync(user);
                return token;
            }

            return null;
       
        }

        public async Task<IdentityResult?> ResetPassword(ResetPasswordModel model)
        {
            var user = await userManagerService.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                var identityResult = await userManagerService.ResetPasswordAsync(user, model.Token, model.Password);
                return identityResult;
            }

            return null;

        }

        public async Task<IdentityResult?> ChangePassword(ClaimsPrincipal User, ChangePasswordModel passwordModel)
        {
            var user = await userManagerService.GetUserAsync(User);
            if (user is not null)
            {
                var identityResult = await userManagerService.ChangePasswordAsync(user, passwordModel.OldPassword, passwordModel.NewPassword);
                if (identityResult.Errors.Any())
                {
                    return identityResult;
                }
                await signInManagerService.RefreshSignInAsync(user);
            }

            return null;

        }

        public async Task<bool> HasPasswordAsync(ClaimsPrincipal user)
        {
            var userDetails = await userManagerService.GetUserAsync(user);
            return await userManagerService.HasPasswordAsync(userDetails);
        }
        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal user)
        {
            return await userManagerService.GetUserAsync(user);
        }
            public async Task<IdentityResult?> AddPasswordAsync(ClaimsPrincipal user, string newPassword)
        {
            var userDetails = await userManagerService.GetUserAsync(user);
            var identityResult=await userManagerService.AddPasswordAsync(userDetails,newPassword);
            if (identityResult.Succeeded)
            {
                await signInManagerService.RefreshSignInAsync(userDetails);
              
            }

            return identityResult;
        }

        public async Task<bool> CheckpasswordAsync(ApplicationUser user, string password)
        {
            return await userManagerService.CheckPasswordAsync(user,password);
        }
        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return await userManagerService
                .GenerateEmailConfirmationTokenAsync(user);
        }
        public async Task SignInAsync(ApplicationUser user)
        {
            await signInManagerService.SignInAsync(user, isPersistent: false);
        }

        public async Task<IdentityResult> AddLoginAsync(ApplicationUser user, UserLoginInfo info)
        {
            return await userManagerService.AddLoginAsync(user, info);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            return await userManagerService.CreateAsync(user);
        }

        public async Task SignOut()
        {
            await signInManagerService.SignOutAsync();
        }


        public async Task<ApplicationUser> IsEmailInUse(string email)
        {
            return await userManagerService.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> FindUserById(string userId)
        {
            return await userManagerService.FindByIdAsync(userId);
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            return await userManagerService.GetClaimsAsync(user);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await userManagerService.GetRolesAsync(user);
        }

        public async Task<IdentityResult> UpdateUser(ApplicationUser user)
        {
            return await userManagerService.UpdateAsync(user);
        }

        public async Task<IdentityResult> RemoveUser(ApplicationUser user)
        {
            return await userManagerService.DeleteAsync(user);
        }


        public async Task<IdentityResult> RemoveFromRoles(ApplicationUser user, IList<string> roles)
        {
            return await userManagerService.RemoveFromRolesAsync(user, roles);
        }

        public async Task<IdentityResult> AddToRoles(ApplicationUser user, List<RoleUserModel> roles)
        {
            return await userManagerService.AddToRolesAsync(user, roles.Where(role=> role.isSelected).Select(role=> role.roleName));
        }

        public async Task<UserClaimsName> GetUserClaims(ApplicationUser user)
        {
            var model = new UserClaimsName
            {
                UserId = user.Id
            };
            var existingUserClaims = await GetClaimsAsync(user);
        
            foreach (Claim claim in ClaimsPrincipal.Current.Claims)
            {
                UserClaimModel userClaim = new()
                {
                    ClaimType = claim.Type
                };

                if(existingUserClaims.Any(claimDetails => claimDetails.Type.Equals(claim.Type) && claimDetails.Value.Equals("true")))
                {
                    userClaim.IsSelected = true;
                }

                model.Claims.Add(userClaim);
            }

            return model;
        }

        public async Task<IdentityResult> RemoveClaims(ApplicationUser user, IList<Claim> claims)
        {
            return await userManagerService.RemoveClaimsAsync(user, claims);
        }

        public async Task<IdentityResult> AddClaims(ApplicationUser user, List<UserClaimModel> claims)
        {
            return await userManagerService.AddClaimsAsync(user, claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false" ))) ;
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemes()
        {
            return await signInManagerService.GetExternalAuthenticationSchemesAsync();
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return signInManagerService.ConfigureExternalAuthenticationProperties(provider,redirectUrl);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await signInManagerService.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo externalLoginInfo)
        {
            return await signInManagerService.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,externalLoginInfo.ProviderKey,isPersistent: false, bypassTwoFactor: true);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await userManagerService.FindByEmailAsync(email);
        }

       
    }
}
