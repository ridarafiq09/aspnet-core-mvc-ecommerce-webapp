using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FlipShop.Core.Entities
{
    [Table("tbl_product")]
    public class Product
    {
       
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Product_ID { get; set; }

        [Required,Column(TypeName = "nvarchar(100)")]
        public string Product_Name { get; init; }

        [Required,Column(TypeName = "nvarchar(500)")]
        public string Product_Description { get; set; }

        [Required,Column(TypeName = "money")]
        public decimal Product_Price { get; set; }
       
        [Required,Column(TypeName = "int")]
        public int Product_SKU { get; set; }
        
        [Required,Column(TypeName = "decimal(4,2)")]
        public decimal Product_Weight { get; set; }
       
        [Required,Column(TypeName = "nvarchar(100)")]
        public string Supplier_Name { get; set; }
       
        [Required, ForeignKey("Category")]
        public int Category_Id { get; set; }

        [Column(TypeName = "datetime")]
        public System.DateTime Publish_Date { get; set; } = DateTime.Now;

        [Column(TypeName = "nvarchar(max)")]
        public string? Product_ImagePath { get; set; }

        [Column(TypeName = "int")]
        public int UnitOnOrder { get; set; } = 0;

        [NotMapped]
        public IFormFile product_Image { get; set; }

        public virtual ICollection<OrderedProductDetails> OrderDetails { get; set; }
    }
}
