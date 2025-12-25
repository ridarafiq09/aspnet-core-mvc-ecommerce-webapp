using FlipShop.Core.Abstractions;
using FlipShop.Core.Abstractions.Accounts;
using FlipShop.Core.Abstractions.Order;
using FlipShop.Core.Abstractions.Products;
using FlipShop.Core.Contract;
using FlipShop.Core.Contract.Cart;
using FlipShop.Core.Entities;
using FlipShop.Infrastructure.Repositories.Products;
using FlipShop.WebApi.Security.Services;
using FlipShop.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Data;
using System.Security.Claims;

namespace FlipShop.WebApi.Areas.Accounts.Controllers
{
    [Area("Account"), Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _genericService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _productService;
        private readonly IOrderRepository _orderService;
        private readonly IUserService _accountService;

        public OrderController(IUnitOfWork unitOfWork,IOrderRepository orderService, IHttpContextAccessor httpContextAccessor, IProductRepository productService,IUserService _accountService)
        {
            this._genericService = unitOfWork;
            this._httpContextAccessor = httpContextAccessor;
            this._productService = productService;
            _orderService = orderService;
           this._accountService = _accountService;
        }
        [HttpGet("Checkout")]
        public async Task<IActionResult> Checkout()
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
            OrderDetailsModel orderDetailsModel = new()
            {
                customerOrderProducts = cartDetailsModel,
                CustomerID = _accountService.GetUserId()

            };
            return View("PlaceOrder", orderDetailsModel);
        }

        [HttpPost("Checkout"), ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder()
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
        
            OrderDetailsModel orderDetailsModel = new()
            {
                customerOrderProducts = CartDetails,
                CustomerID = _accountService.GetUserId()
        };


            var OrderModelUpdated = TryUpdateModelAsync<OrderDetailsModel>(orderDetailsModel);
            var errors = ModelState.Select(x => x.Value.Errors).ToList();
            if (OrderModelUpdated.Result.Equals(true))
            {
                _orderService.PlaceOrder(orderDetailsModel);
                return View("OrderConfimation", orderDetailsModel);
            }
             
            return View(orderDetailsModel);
        }


    }
}
