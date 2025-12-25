using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Entities
{
    public class OrderedProductDetails
    {
        public int Quantity { get; set; }
        public int ProductID { get; set; }
        public int Order_ID { get; set; }
        public decimal Unit_Price { get; set; }

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
