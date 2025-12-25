using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Contract.Orders
{
    public class OrderInfo
    {
        public decimal TotalAmount { get; set; }
        public float DiscountByCoupon { get; set; }
        public PaymentType Payment_Type { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingFee { get; set; }
    }
}
