using FlipShop.Core.Entities;
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

namespace FlipShop.Core.Contract
{
    public class ProductDTO 
    {
      
        [HiddenInput(DisplayValue = false)]
        public int Product_ID { get; set; }


        [Required(ErrorMessage = "The 'Name' field is Required"), Display(Name = "Product Name")]
        public string Product_Name { get; set; }


        [Required(ErrorMessage = "The Product Description must not be empty"), Display(Name = "Product Description")]
        public string Product_Description { get; set; }


        [Required(ErrorMessage = "The Price must not be empty"), Display(Name = "Product Price (OMR)")]
        [Range(minimum: 1, maximum: double.MaxValue), DataType(DataType.Currency)]
        public decimal Product_Price { get; set; }


        [Required(ErrorMessage = "The SKU must not be empty"), Display(Name = "Product Stock")]
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int Product_SKU { get; set; }


        [Required(ErrorMessage = "The Product Weight must not be empty"), Display(Name = "Product Weight (Kg)")]
        [Range(minimum: 0.1, maximum: double.MaxValue, ErrorMessage = "Product weight must be greater than 0.1")]
        public decimal Product_Weight { get; set; }


        [Required(ErrorMessage = "The 'Supplier Name' field is Required"), Display(Name = "Supplier Name")]
        public string Supplier_Name { get; set; }


        [Required(ErrorMessage = "Please Select a category. If required category not present, Contact the admin"), Display(Name = "Category")]
        public int Category_Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss tt}")]
        public Nullable<System.DateTime> Publish_Date { get; set; }

        [Required, Display(Name = "Select an Image")]
        public IFormFile product_Image { get; set; }
        public string? Product_ImagePath { get; set; }

        [Display(Name = "Units On Order")]
        public int UnitOnOrder { get; set; }
    }
}
