using FlipShop.Core.Abstractions.Accounts;
using FlipShop.Core.Contract.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Security.Claims;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace FlipShop.WebApi.Areas.User.Controllers
{
    //Not Displayed on URL
    [Area("Account")]
    public class AuthController : Controller
    {
        private readonly IAccountRepository _accountService;

        public AuthController(IAccountRepository _accountService)
        {
            this._accountService = _accountService;
        }

        [ViewData]
        public string Title { get; set; } = "Account";


        [HttpGet("Signup")]
        public ViewResult SignUp() => View();


        [HttpPost, Route("Signup"), ActionName("SignUp"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Account user, string? ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                IdentityResult identityResult = await _accountService.AddUser(user);
                if (identityResult is not null and { Succeeded: true })
                {
                    //redirect to page to say ka login succesful but please verify
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        //To prevent open redirect attack (security hole)
                        return LocalRedirect(ReturnUrl);
                    }

                    ModelState.Clear();
                    return RedirectToAction("", "Home", new { area = "" });
                }

                if (identityResult?.Errors is not null)
                {
                    foreach (var error in identityResult.Errors) ModelState.AddModelError(String.Empty, error.Description);
                }

            }
            return View(user);
        }


        [HttpGet("SignIn")]
        public async Task<IActionResult> SignIn(string returnUrl)
        {
            Account account = new()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _accountService.GetExternalAuthenticationSchemes()).ToList()
            };
            return View(account);
        }


        [HttpPost, Route("SignIn"), ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn([Bind(include: "UserName, Password, RememberMe,ReturnUrl")] Account user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                SignInResult result = await _accountService.SignIn(user);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(user.ReturnUrl) && Url.IsLocalUrl(user.ReturnUrl))
                    {
                        //To prevent open redirect attack (security hole)
                        return LocalRedirect(user.ReturnUrl);
                    }
                    ModelState.Clear();
                    return RedirectToAction("", "Home", new { area = "" });
                }

                if (result.IsLockedOut)
                {
                    return View("lockoutview");
                }
            }

            ModelState.AddModelError(String.Empty, "Invalid Login Attempt");
            return View(user);
        }

        [HttpPost("ExternalLogin")]
        public IActionResult ExternalLoginProviders(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _accountService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet("ExternalLogin")]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            Account account = new()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _accountService.GetExternalAuthenticationSchemes()).ToList()
            };

            if (remoteError is not null)
            {
                ModelState.AddModelError("", $"Error from external provider : {remoteError}");
                return View("Login", account);
            }

            var externalLoginInfo = await _accountService.GetExternalLoginInfoAsync();
            if (externalLoginInfo is null)
            {
                ModelState.AddModelError("", $"Error loading external login information");
                return View("Login", account);
            }

            var signInResult = await _accountService.ExternalLoginSignInAsync(externalLoginInfo);
            if (signInResult.Succeeded) return LocalRedirect(returnUrl);
            else
            {
                var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                if (email is not null)
                {
                    var user = await _accountService.FindByEmailAsync(email);

                    if (user is null)
                    {
                        user = new ApplicationUser()
                        {
                            UserName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Name),
                            Email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        await _accountService.CreateAsync(user);
                    }

                    await _accountService.AddLoginAsync(user, externalLoginInfo);
                    await _accountService.SignInAsync(user);

                    return LocalRedirect(returnUrl);
                }

                ViewBag.ErrorTitle = $"Email claim not received from: {externalLoginInfo.LoginProvider}";
                ViewBag.ErrorMessage = $"Please Contact support on";

                return View("Error");
            }

        }

        [HttpGet("AddPasswordForExternalProvider")]
        public async Task<IActionResult> AddPasswordForExternalProviders()
        {
            var userHasPassword = await _accountService.HasPasswordAsync(User);
            if (userHasPassword)
            {
                return RedirectToAction("ChangePassword");
            }
            return View();
        }

        [HttpPost("AddPasswordForExternalProvider")]
        public async Task<IActionResult> AddPasswordForExternalProviders(string newpassword)
        {
            if (ModelState.IsValid)
            {
                var identityResult = await _accountService.AddPasswordAsync(User, newpassword);
                if (identityResult is not null)
                {
                    foreach (var error in identityResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
                    return View();
                }
                return View();
            }
            return View();
        }

        [HttpPost("Logout"), Authorize]
        public new async Task<IActionResult> SignOut()
        {
            await _accountService.SignOut();
            return RedirectToAction("", "Home", new { area = "" });
        }


        //Remote Email Validation (asynchronously)
        [AcceptVerbs("Get", "Post"), Route("ValidateEmail")]
        public async Task<JsonResult> EmailAlreadyInUse(string Email)
        {
            if (await _accountService.IsEmailInUse(Email) is null) return Json(true);

            return Json($"Email {Email} is already in use");
        }

        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword() => View();


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            //better to use viewmodel with data annonation attr
            if (ModelState.IsValid)
            {
                var token = await _accountService.ForgotPassword(email);

                if (token is null)
                {
                    ModelState.AddModelError(String.Empty, "User doesn't exist");
                    return View();
                }

                var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = email, token = token }, Request.Scheme);
                //if you have an account with us, we have sent an email with the instructions
                //to reset your password
                return View("ForgotPasswordConfirmation");
            }
            return View();
        }


        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId is null || token is null)
            {
                return RedirectToAction("", "Home", new { area = "" });
            }

            var user = await _accountService.FindUserById(userId);
            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with {userId} not found";
                return View("NotFound");
            }

            var userValidatedResult = await _accountService.ConfirmEmailAsync(user, token);
            if (userValidatedResult.Succeeded)
            {
                //Thanks for confirming email page
                return View();
            }

            return View();
        }

        [HttpGet("ResetPassword"), Authorize]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            if (token is null || email is null)
            {
                return RedirectToAction("", "Home", new { area = "" });
            }
            return View();
        }

        [HttpPost("ResetPassword"), Authorize]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var identityResult = await _accountService.ResetPassword(model);
                if (identityResult is not null)
                {
                    if (identityResult.Succeeded)
                    {
                        //code if user locked out and now he comes to change password even then if
                        //can't login so can use code written here to override this behaviur
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in identityResult.Errors) ModelState.AddModelError("", error.Description);
                    return View(model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }


        [HttpGet("ChangePassword"), Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var userHasPassword = await _accountService.HasPasswordAsync(User);
            if (!userHasPassword)
            {
                return RedirectToAction("AddPassword");
            }
            return View();
        }


        [HttpPost("ChangePassword"), Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel passwordModel)
        {
            if (ModelState.IsValid)
            {
                var identityResult = await _accountService.ChangePassword(User, passwordModel);
                if (identityResult is not null)
                {
                    foreach (var error in identityResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
                    return View(passwordModel);
                }
                //howgaya change
                return View();
            }
            return View(passwordModel);
        }


        [HttpGet("Account/AccessDenied")]
        public IActionResult AccessDenied() => View();


        [HttpGet, Authorize]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _accountService.FindUserById(userId);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with {userId} not found";
                return View("NotFound");
            }

            var userModel = new EditUserModel
            {
                Email = user.Email,
                UserName = user.UserName,
                Id = user.Id,
                Claims = (await _accountService.GetClaimsAsync(user)).Select(claim => claim.ValueType + " : " + claim.Value).ToList(),
                Roles = await _accountService.GetRolesAsync(user)
            };
            return View(userModel);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> EditUser(EditUserModel userModel)
        {
            var user = await _accountService.FindUserById(userModel.Id);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with {userModel.Id} not found";
                return View("NotFound");
            }
            user.Email = userModel.Email;
            user.UserName = userModel.UserName;

            var result = await _accountService.UpdateUser(user);
            if (result.Succeeded) return RedirectToAction("ListUsers");

            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);

            return View(userModel);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _accountService.FindUserById(userId);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with {userId} not found";
                return View("NotFound");
            }

            var result = await _accountService.RemoveUser(user);
            if (result.Succeeded) return RedirectToAction("ListUsers");

            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);

            return View("ListUsers");
        }


        public JsonResult AutocompleteInput(string term)
        {
            return Json(term);
        }


    }
}
