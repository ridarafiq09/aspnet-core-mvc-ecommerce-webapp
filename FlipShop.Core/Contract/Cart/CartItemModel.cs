using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FlipShop.Core.Contract.Cart
{
    public class CartItemModel
    {
        public int Product_ID { get; set; }

        [Required, Display(Name = "Product Name")]
        public string Product_Name { get; set; }

        [Required, Display(Name = "Product Description")]
        public string Product_Description { get; set; }

        [Required, Display(Name = "Product Price"), Range(minimum: 1, maximum: double.MaxValue)]
        public decimal Product_Price { get; set; }

        [Required, Display(Name = "Product Quanity"), Range(minimum: 1, maximum: int.MaxValue)]
        public int Product_quantity { get; set; }
        public string Product_ImagePath { get; set; }
    }
}
