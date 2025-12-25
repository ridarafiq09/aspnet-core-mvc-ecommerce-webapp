using FlipShop.Core.Contract.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Entities
{
    public class Order
    {
        public Order()
        {
            this.OrderAddress = new OrderAddress();
            this.OrderDetails = new List<OrderedProductDetails>();
            this.UserTransactionDetails = new UserTransactionDetails();
        }

        public int OrderID { get; set; }
        public byte OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public System.DateTime Order_Date { get; set; }
        public Nullable<System.DateTime> Due_Date { get; set; }
        public float DiscountByCoupon { get; set; }
        public string CustomerID { get; set; }
        public byte Payment_Type { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingFee { get; set; }
        public int orderAddressId { get; set; }

        public int userTransactionId { get; set; }
        public virtual ApplicationUser Customer { get; set; }
        public virtual OrderAddress OrderAddress { get; set; }
        public virtual ICollection<OrderedProductDetails> OrderDetails { get; set; }
        public virtual UserTransactionDetails UserTransactionDetails { get; set; }
    }
}
