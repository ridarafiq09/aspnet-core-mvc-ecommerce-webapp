using FlipShop.Core.Abstractions.Accounts;
using FlipShop.Core.Contract.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FlipShop.WebApi.Areas.Accounts.Controllers
{

    [Area("Account"), Route("admin/[action]"), Authorize(Roles = "Admin",Policy = "AlterRolePolicy")]
    public class AdministrationController : Controller
    {
        private readonly IAdministration _administrationService;

        public AdministrationController(IAdministration _administrationService)
        {
            this._administrationService = _administrationService;
        }

        [HttpGet]
        public ViewResult CreateRole() => View();


        [HttpPost]
        public async Task<IActionResult> CreateRole([Bind(include: "roleName")] RoleModel role)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _administrationService.CreateRole(role.roleName);

                if (result.Succeeded)
                    return RedirectToAction("", "Home", new { area = "" });

                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(role);
        }

        [HttpGet]
        public ViewResult ListRoles()
        {
            IQueryable<IdentityRole> roles = _administrationService.ListRoles();
            return View(roles);
        }

        //1 to many relationship between role and user thats why roleid not in model
        [HttpGet, Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> UpdateRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await _administrationService.FindRoleById(roleId);

            if (role is null)
            {
                ViewBag.ErrorMessage = $"Role with {roleId} not found";
                return View("NotFound");
            }

            var model = new RoleModel
            {
                roleId = role.Id,
                roleName = role.Name,
                Users = await _administrationService.UsersInRole(role.Name)
            };
            return View(model);
        }

        [HttpPost, Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> UpdateRole(RoleModel roleModel)
        {

            IdentityRole role = await _administrationService.FindRoleById(roleModel.roleId);

            if (role is null)
            {
                ViewBag.ErrorMessage = $"Role with {roleModel.roleId} not found";
                return View("NotFound");
            }

            IdentityResult result = await _administrationService.EditRole(role);
            if (result.Succeeded)
                return RedirectToAction("", "Home", new { area = "" });


            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

            return View(roleModel);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateUsersInRole(string roleId)
        {

            IdentityRole role = await _administrationService.FindRoleById(roleId);
            if (role is null)
            {
                ViewBag.ErrorMessage = $"Role with {roleId} not found";
                return View("NotFound");
            }

            IEnumerable<UserRoleModel> applicationUsers = await _administrationService.AllUsersWithRole(role.Name);
            return View(applicationUsers);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUsersInRole(List<UserRoleModel> users, string roleId)
        {
            var role = await _administrationService.FindRoleById(roleId);
            if (role is null)
            {
                ViewBag.ErrorMessage = $"Role with {roleId} not found";
                return View("NotFound");
            }

            await _administrationService.UpdateUsersRole(users, role.Name);

            return RedirectToAction("UpdateUsersInRole", new { roleId = roleId });
        }

        [HttpGet]
        public ViewResult ListUsers()
        {
            var users = _administrationService.GetAllUsers();
            return View(users);
        }

        [HttpPost, Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string roleId)

        {
            var role = await _administrationService.FindRoleById(roleId);

            if (role is null)
            {
                ViewBag.ErrorMessage = $"Role with {roleId} not found";
                return View("NotFound");
            }

            var result = await _administrationService.RemoveRole(role);
            if (result.Succeeded) return RedirectToAction("ListRoles");

            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);

            return View("ListRoles");
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId, IAccountRepository _accountRepository)
        {

            ViewBag.userId = userId;
            var user = await _accountRepository.FindUserById(userId);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"Role with {userId} not found";
                return View("NotFound");
            }

            var userListWithRoles = await _administrationService.RolesofUser(user);

            return View(userListWithRoles);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<RoleUserModel> roleUserModels, string userId, IAccountRepository _accountRepository)
        {

            var user = await _accountRepository.FindUserById(userId);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with {userId} not found";
                return View("NotFound");
            }

            var roles = await _accountRepository.GetRolesAsync(user);
            var removeRolesResult = await _accountRepository.RemoveFromRoles(user, roles);


            if (!removeRolesResult.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View();
            }

            var addRolesResult = await _accountRepository.AddToRoles(user, roleUserModels);

            if (!removeRolesResult.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View();
            }

            return RedirectToAction("EditUser", new { Id = userId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId, IAccountRepository _accountRepository)
        {
            var user = await _accountRepository.FindUserById(userId);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with {userId} not found";
                return View("NotFound");
            }

            var userClaimsList = await _accountRepository.GetUserClaims(user);

            return View(userClaimsList);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(List<UserClaimModel> userClaimModels, string userId, IAccountRepository _accountRepository)
        {
            var user = await _accountRepository.FindUserById(userId);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with {userId} not found";
                return View("NotFound");
            }

            var userClaimsList = await _accountRepository.GetClaimsAsync(user);
            var removeClaimsResult = await _accountRepository.RemoveClaims(user, userClaimsList);


            if (!removeClaimsResult.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View();
            }

            var addClaimsResult = await _accountRepository.AddClaims(user, userClaimModels);

            if (!addClaimsResult.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View();
            }

            return RedirectToAction("EditUser", new { Id = userId });
        }


    }
}
