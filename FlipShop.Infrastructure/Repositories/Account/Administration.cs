using FlipShop.Core.Abstractions.Accounts;
using FlipShop.Core.Contract.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Infrastructure.Repositories.Accounts
{
    public class Administration : IAdministration
    {
        private RoleManager<IdentityRole> roleManagerService;
        private UserManager<ApplicationUser> userManagerService;

        public Administration(RoleManager<IdentityRole> roleManagerService, UserManager<ApplicationUser> userManagerService)
        {
            this.roleManagerService = roleManagerService;
            this.userManagerService = userManagerService;
        }

        public async Task<IdentityResult> CreateRole(string RoleName)
        {
            IdentityRole identityRole = new()
            {
                Name = RoleName,
            };

            return await roleManagerService.CreateAsync(identityRole);
        }

        public IQueryable<IdentityRole> ListRoles()
        {
            return roleManagerService.Roles;
        }

        public async Task<IdentityRole> FindRoleById(string roleId)
        {
            return await roleManagerService.FindByIdAsync(roleId);
        }

        public async Task<IdentityResult> EditRole(IdentityRole role)
        {
            return await roleManagerService.UpdateAsync(role);
        }
        public IQueryable<ApplicationUser> GetAllUsers()
        {
            return userManagerService.Users;
        }

        public Task<IdentityResult> RemoveRole(IdentityRole role)
        {
            return roleManagerService.DeleteAsync(role);
        }

        public async Task<List<string>> UsersInRole(string role)
        {
            List<ApplicationUser> UsersList = new();

            foreach (var user in userManagerService.Users)
            {
                if (await userManagerService.IsInRoleAsync(user, role))
                    UsersList.Add(user);
            }

            return UsersList.Select(user => user.UserName).ToList();

        }

        public async Task<List<UserRoleModel>> AllUsersWithRole(string roleName)
        {
            List<UserRoleModel> applicationUsers = new();

            foreach (var user in userManagerService.Users)
            {
                if (await userManagerService.IsInRoleAsync(user, roleName))
                {
                    applicationUsers.Add(new UserRoleModel
                    {
                        userId = user.Id,
                        userName = user.UserName,
                        isSelected = true
                    });
                }
                else
                {
                    applicationUsers.Add(new UserRoleModel
                    {
                        userId = user.Id,
                        userName = user.UserName,
                        isSelected = false
                    });
                }
            }
            return applicationUsers;

        }


        public async Task UpdateUsersRole(List<UserRoleModel> users, string roleName)
        {

            for (int i = 0; i < users.Count - 1; i++)
            {
                ApplicationUser userInfo = await userManagerService.FindByIdAsync(users[i].userId);
                IdentityResult result = null;

                if (users[i].isSelected && !(await userManagerService.IsInRoleAsync(userInfo, roleName)))
                {
                    result = await userManagerService.AddToRoleAsync(userInfo, roleName);
                }
                else if (!(users[i].isSelected) && await userManagerService.IsInRoleAsync(userInfo, roleName))
                {
                    result = await userManagerService.RemoveFromRoleAsync(userInfo, roleName);
                }
                else continue;

                if (result.Succeeded)
                {
                    if (i < (users.Count - 1))
                        continue;
                    else
                        return;
                }
            }

        }

        public async Task<List<RoleUserModel>> RolesofUser(ApplicationUser user)
        {
            List<RoleUserModel> userInRoles = new();

            foreach (var role in roleManagerService.Roles)
            {
                if (await userManagerService.IsInRoleAsync(user, role.Name))
                {
                    userInRoles.Add(new RoleUserModel
                    {
                        roleId = role.Id,
                        roleName = role.Name,
                        isSelected = true
                    });
                }
                else
                {
                    userInRoles.Add(new RoleUserModel
                    {
                        roleId = role.Id,
                        roleName = role.Name,
                        isSelected = false
                    });
                }
            }
            return userInRoles;
        }

    
    }
}
