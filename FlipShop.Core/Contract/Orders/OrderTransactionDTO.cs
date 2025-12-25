using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FlipShop.Core.Contract.Orders
{
    public class OrderTransactionDTO
    {

        [Display(Name = "Name on Card"), Required(ErrorMessage = "Card Name must not be empty")]
        public string NameOnCard { get; set; }
        [Required(ErrorMessage = "CVV must not be empty"), Display(Name = "CVV")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV must be of 3 characters")]
        public short CVV { get; set; }
        [Display(Name = "Expiration Date"), DataType(DataType.Date), Required(ErrorMessage = "Please set the card expiration date")]
        public System.DateTime ExpirationDate { get; set; }

        [Display(Name = "Account Number"), Required(ErrorMessage = "Account Number must not be empty")]
        [RegularExpression(@"^4[0-9]{12}(?:[0-9]{3})?$", ErrorMessage = "Account Number can be of 13 or 16 characters")]
        public long CardNumber { get; set; }
    }
}
