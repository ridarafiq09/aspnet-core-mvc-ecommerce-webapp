using AutoMapper;
using FlipShop.Core.Abstractions;
using FlipShop.Core.Abstractions.Products;
using FlipShop.Core.Contract.Cart;
using FlipShop.Core.Entities;
using FlipShop.Infrastructure.Repositories;
using FlipShop.Infrastructure.Repositories.Products;
using FlipShop.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FlipShop.WebApi.Areas.Products.Controllers
{
    [Area("Product")]
    public class ProductFilterController : Controller
    {
        private readonly IUnitOfWork unitofwork;
        private readonly IProductRepository _productService;

        public ProductFilterController(IUnitOfWork unitOfWork, IProductRepository productService)
        {
            this.unitofwork = unitOfWork;
            this._productService = productService;
        }


        [HttpGet("Products")]
        public async Task<IActionResult> GetAll(string searchitem, string sortOrder, int? pageNo)
        {
            //To Preserve the complete filter query
            ViewBag.SortOrder = sortOrder ??= String.Empty;
            ViewBag.Searchitem = searchitem ??= String.Empty;

            var products = _productService.SearchProducts(searchitem, sortOrder);
            return View("ProductsPage", await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNo ?? 1, 6));
        }


        [HttpGet("Product/Details/{id:int}")]
        public IActionResult GetById(int id)
        {
            Product product = unitofwork.ProductRepository.GetById(id);
            if (product?.Product_ID is null) return NotFound();

            if (TempData[AppStringVariables.tempDataCartItems] is not null)
            {
                ViewBag.UserCart = JsonConvert.DeserializeObject<List<CartItemCookie>>((string)TempData[AppStringVariables.tempDataCartItems]);
                return View("ShowProduct", product);
            }


            return View("ShowProduct", product);
        }

        [HttpGet, Route("GetCategoryFilteredProducts")]
        public PartialViewResult GetCategoryFilteredProducts(int categoryId)
        {
            var products = _productService.GetProductsByCategory(categoryId);
            return PartialView("_HomeProductCard", products);
        }

    }
}
