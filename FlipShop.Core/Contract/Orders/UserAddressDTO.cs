using FlipShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Contract.Orders
{
    public class UserAddressDTO
    {

        [Required(ErrorMessage = "Address must not be empty"), Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address Line 2 (Optional)")]
        public string AddressLine2 { get; set; }
        [Required(ErrorMessage = "City must not be empty"), Display(Name = "City")]
        public string City { get; set; }
        [Required(ErrorMessage = "State must not be empty"), Display(Name = "State")]

        public string State { get; set; }
        [Required(ErrorMessage = "Postal Code must not be empty"), DataType(DataType.PostalCode), Display(Name = "Postal Code")]
        public short Postal { get; set; }
        [Required(ErrorMessage = "Country must not be empty"), Display(Name = "Country")]
        public string Country { get; set; }
    }
}
