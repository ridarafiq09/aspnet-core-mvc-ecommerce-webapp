using FlipShop.Core.Contract.Cart;
using FlipShop.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace FlipShop.WebApi.Components
{
    public class CartBadgeViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartBadgeViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartCookie = _httpContextAccessor.HttpContext?.Request.Cookies[AppStringVariables.cartItemsCookieName];
            if (cartCookie is null)
            {
                return View(0);
            }
            TempData[AppStringVariables.tempDataCartItems] = cartCookie;
            var cartItemlist = cartCookie.FromJson<List<CartItemCookie>>();

            return View(cartItemlist.Count);
        }
    }
}
