using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Contract.Cart
{
    public class CartDetails
    {
        public CartDetails()
        {
            CartItems = new List<CartItemModel>();
        }

        public IEnumerable<CartItemModel> CartItems { get; set; }

        public decimal SubTotal { get; set; }

        public decimal ShippingFee { get; set; }

        public decimal Tax { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
