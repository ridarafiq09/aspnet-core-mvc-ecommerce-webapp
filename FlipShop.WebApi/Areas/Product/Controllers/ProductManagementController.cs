using FlipShop.Core.Abstractions;
using FlipShop.Core.Contract;
using FlipShop.WebApi.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using FlipShop.Core.Entities;
using AutoMapper;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using FlipShop.WebApi.Utilities;

namespace FlipShop.WebApi.Areas.Products.Controllers
{

    [Area("Product"), Route("[controller]/[action]"), Authorize(Policy = "EditProductPolicy")]
    public class ProductManagementController : Controller
    {
        private readonly IUnitOfWork _productService;
        private readonly IWebHostEnvironment webhostenvironment;
        private readonly IMapper mapper;

        public ProductManagementController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, IMapper mapper)
        {
            this._productService = unitOfWork;
            webhostenvironment = hostEnvironment;
            this.mapper = mapper;
        }

        
        [HttpGet]
        public IActionResult Add() => View("AddProduct");


        [HttpPost]
        public async Task<IActionResult> Add(ProductDTO product)
        {
            if (ModelState.IsValid && product.product_Image is not null and { Length: > 0 })
            {
                string? imagePath = await HandleImage.AddImageToServer(product.product_Image, product, HandleImage.productImageFolder, webhostenvironment);
                if (imagePath is null)
                {
                    ModelState.AddModelError(String.Empty, "Error: Something went wrong. Please try again");
                    return View("AddProduct");
                }
                product.Product_ImagePath = imagePath;
                product.Publish_Date = DateTime.Now;
                var productDetails = mapper.Map<Product>(product);
                _productService.ProductRepository.Add(productDetails);
                await _productService.SaveChanges();
                ModelState.Clear();

                return RedirectToAction("", "", new { area = "" });
            }


            return View("AddProduct", product);
        }

        [HttpGet("{id}:int")]
        public IActionResult Update(int id)
        {

            Product product = _productService.ProductRepository.GetById(id);

            if (product?.Product_ID is null) return RedirectToAction("NotFound", "error", new { area = "" });

            return View(mapper.Map<ProductDTO>(product));
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDTO product)
        {
            if (!ModelState.IsValid) return View();

            if (product.product_Image is not null)
            {
                if (product.Product_ImagePath is not null)
                {
                    string existingProductImgPath = Path.Combine(webhostenvironment.WebRootPath + HandleImage.productImageFolder, product.Product_ImagePath);
                    System.IO.File.Delete(existingProductImgPath);

                }

                string? imagePath = await HandleImage.AddImageToServer(product.product_Image, product, HandleImage.productImageFolder, webhostenvironment);
                if (imagePath is null)
                {
                    ModelState.AddModelError(String.Empty, "Error: Something went wrong. Please try again");
                    return View();
                }
                product.Product_ImagePath = imagePath;
                product.Publish_Date = DateTime.Now;


            }
            _productService.ProductRepository.Update(mapper.Map<Product>(product));
            await _productService.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("Product", "Details", new { productId = product.Product_ID });
        }



        [HttpPost("{id:int}")]
        public IActionResult Remove(int id)
        {
            var productFound = _productService.ProductRepository.GetById(id);

            if (productFound?.Product_ImagePath is null) return RedirectToAction("ServerError", "error", new { area = "" });

            var product = new Product()
            {
                Product_ID = id
            };

            string existingProductImgPath = Path.Combine(webhostenvironment.WebRootPath + HandleImage.productImageFolder, product.Product_ImagePath);
            System.IO.File.Delete(existingProductImgPath);
            _productService.ProductRepository.Remove(product);
            _productService.SaveChanges();
            return View("index");
        }

    }
}
