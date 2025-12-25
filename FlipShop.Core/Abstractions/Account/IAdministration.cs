using FlipShop.Core.Contract.Administration;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Abstractions.Accounts
{
    public interface IAdministration
    {
        Task<IdentityResult> CreateRole(string RoleName);
        IQueryable<IdentityRole> ListRoles();
        Task<IdentityRole> FindRoleById(string roleId);
        Task<IdentityResult> EditRole(IdentityRole role);
        Task<List<string>> UsersInRole(string role);
        IQueryable<ApplicationUser> GetAllUsers();
        Task<IdentityResult> RemoveRole(IdentityRole role);
        Task<List<UserRoleModel>> AllUsersWithRole(string role);
        Task UpdateUsersRole(List<UserRoleModel> users, string roleName);
        Task<List<RoleUserModel>> RolesofUser(ApplicationUser user);

    }
}
