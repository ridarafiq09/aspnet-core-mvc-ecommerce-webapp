using System.ComponentModel.DataAnnotations;

namespace FlipShop.WebApi.Utilities
{
    public class ValidateEmailDomain : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidateEmailDomain(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }

        public override bool IsValid(object? value)
        {
            if (value is null) return false;

            string[] userInput = value.ToString().Split('@');

            return userInput[1].ToUpper().Equals(allowedDomain.ToUpper());

        }
    }

    //[ValidateEmailDomain(allowedDomain:"gmail.com",ErrorMessage ="Email domain must be gmail.com")]
}
