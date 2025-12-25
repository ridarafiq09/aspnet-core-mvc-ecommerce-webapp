using FlipShop.Core.Contract.Cart;
using FlipShop.Core.Contract.Orders;
using FlipShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Contract
{
    public class OrderDetailsModel
    {
        public OrderDetailsModel()
        {
            this.customerOrderProducts = new CartDetails();
            this.address = new UserAddressDTO();
            this.transactionDetails = new OrderTransactionDTO();
            this.orderdetails = new OrderInfo();
        }

        public CartDetails customerOrderProducts { get; set; }
        public UserAddressDTO address { get; set; }

        public OrderTransactionDTO transactionDetails { get; set; }

        public OrderInfo orderdetails { get; set; }

        public string CustomerID { get; set; }
    }
    public enum PaymentType
    {
        CreditCard,
        DebitCard,
        PayPal
    }
}
