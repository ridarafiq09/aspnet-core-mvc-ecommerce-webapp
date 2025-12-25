using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Contract.Administration
{
    public class Account
    {
        [Required, DataType(DataType.EmailAddress), EmailAddress,
         Remote(action: "", controller: "ValidateEmail", areaName: "")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The 'Password' field is required"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password"), DataType(DataType.Password),
         Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "The 'Name' field is required"), Display(Name = "User Name"),
         ]
        public string UserName { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        //configured authentication provider list
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
