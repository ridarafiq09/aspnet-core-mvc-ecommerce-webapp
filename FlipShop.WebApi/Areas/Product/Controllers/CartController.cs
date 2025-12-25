using FlipShop.Core.Abstractions;
using FlipShop.Core.Abstractions.Products;
using FlipShop.Core.Contract.Cart;
using FlipShop.Core.Entities;
using FlipShop.Infrastructure.Repositories.Products;
using FlipShop.WebApi.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;
using static System.Collections.Specialized.BitVector32;

namespace FlipShop.WebApi.Areas.Products.Controllers
{
    [Area("Product"), Route("[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _genericService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _productService;

        public CartController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IProductRepository productService)
        {
            this._genericService = unitOfWork;
            this._httpContextAccessor = httpContextAccessor;
            this._productService = productService;
        }

        public async Task<IActionResult> ViewCart()
        {
            var cartCookie = _httpContextAccessor.HttpContext?.Request.Cookies[AppStringVariables.cartItemsCookieName];

            if (cartCookie is null) return View("EmptyCart");

            var cartItems = await _productService.GetCartItems(cartCookie.FromJson<List<CartItemCookie>>());

            if (cartItems.Count().Equals(0)) return View("EmptyCart");

            CartDetails cartDetailsModel = new()
            {
                CartItems = cartItems
            };

            var CartDetails = _productService.CalculateBill(cartDetailsModel);
            return View("ShowCart", CartDetails);

        }

        public IActionResult AddItem(int productId, int quantity)
        {
            var cartCookie = _httpContextAccessor.HttpContext?.Request.Cookies[AppStringVariables.cartItemsCookieName];

            var product = _genericService.ProductRepository.GetById(productId);
            if (product?.Product_ID is null) return RedirectToAction("NotFound", "error", new { area = "" });

            var newCartItem = new CartItemCookie()
            {
                product_id = productId,
                quantity = quantity
            };
            if (cartCookie is null)
            {
                List<CartItemCookie> newCartItemsList = new();
                newCartItemsList.Add(newCartItem);
                CookieConfiguration(newCartItemsList);
                return RedirectToAction("", "Home", new { area = "" });
            }
            var cartItemsList = cartCookie.FromJson<List<CartItemCookie>>();
            if (!(cartItemsList.Any(item => item.product_id.Equals(newCartItem.product_id))))
            {
                cartItemsList.Add(newCartItem);
                CookieConfiguration(cartItemsList);
                return RedirectToAction("", "Home", new { area = "" });
            }

            return RedirectToAction("", "Home", new { area = "" });
        }

        public IActionResult RemoveItem(int productId)
        {
            var cartCookie = _httpContextAccessor.HttpContext?.Request.Cookies[AppStringVariables.cartItemsCookieName];

            if (cartCookie is null) return View("EmptyCart");
            var cartItemsList = cartCookie.FromJson<List<CartItemCookie>>();
            int cartItemIndex = cartItemsList.FindIndex(item => item.product_id.Equals(productId));

            if (cartItemIndex.Equals(-1)) return RedirectToAction("", "Home", new { area = "" });

            cartItemsList.RemoveAt(cartItemIndex);
            if (cartItemsList.Count.Equals(0))
            {
                _httpContextAccessor.HttpContext?.Response.Cookies.Delete(AppStringVariables.cartItemsCookieName);
                return RedirectToAction("ViewCart", "Cart", new { area = "" });
            }
            CookieConfiguration(cartItemsList);
            return RedirectToAction("ViewCart", "Cart", new { area = "" });
        }

        private void CookieConfiguration(List<CartItemCookie> cartItemslist)
        {
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddMonths(1);
            // cookieOptions.Secure = true;
            cookieOptions.SameSite = SameSiteMode.Strict;
            cookieOptions.HttpOnly = false;
            HttpContext.Response.Cookies.Append("cartItems", JsonConvert.SerializeObject(cartItemslist), cookieOptions);
        }
    }
}
