using FlipShop.Core.Contract.Administration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Abstractions.Accounts
{
    public interface IAccountRepository : IRepository<ApplicationUser>
    {
        Task<IdentityResult> AddUser(Account user);
        Task<SignInResult> SignIn(Account user);
        Task SignOut();
        Task<ApplicationUser> IsEmailInUse(string email);
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal user);
        Task<bool> CheckpasswordAsync(ApplicationUser user, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<string?> ForgotPassword(string email);
        Task<IdentityResult?> ResetPassword(ResetPasswordModel model);
        Task<IdentityResult?> ChangePassword(ClaimsPrincipal User, ChangePasswordModel passwordModel);
        Task<ApplicationUser> FindUserById(string userId);
        Task<bool> HasPasswordAsync(ClaimsPrincipal user);
        Task<IdentityResult?> AddPasswordAsync(ClaimsPrincipal user, string newPassword);
        Task<IList<Claim>> GetClaimsAsync(ApplicationUser user);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IdentityResult> UpdateUser(ApplicationUser user);
        Task<IdentityResult> RemoveUser(ApplicationUser user);

        Task<IdentityResult> RemoveFromRoles(ApplicationUser user, IList<string> roles);
        Task<IdentityResult> AddToRoles(ApplicationUser user, List<RoleUserModel> roles);

        Task<UserClaimsName> GetUserClaims(ApplicationUser user);
        Task<IdentityResult> RemoveClaims(ApplicationUser user, IList<Claim> claims);
        Task<IdentityResult> AddClaims(ApplicationUser user, List<UserClaimModel> claims);

        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemes();

        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo externalLoginInfo);

        Task<ApplicationUser> FindByEmailAsync(string email);
        Task SignInAsync(ApplicationUser user);
        Task<IdentityResult> AddLoginAsync(ApplicationUser user, UserLoginInfo info);
        Task<IdentityResult> CreateAsync(ApplicationUser user);
    }
}
