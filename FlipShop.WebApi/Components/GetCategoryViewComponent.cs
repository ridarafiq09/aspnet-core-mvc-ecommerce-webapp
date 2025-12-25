using FlipShop.Core.Abstractions.Products;
using FlipShop.Infrastructure.Repositories.Products;
using Microsoft.AspNetCore.Mvc;

namespace FlipShop.WebApi.Components
{
    public class GetCategoryViewComponent : ViewComponent
    {

        private readonly IProductRepository _productService;

        public GetCategoryViewComponent(IProductRepository productService)
        {
            this._productService = productService;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _productService.GetCategories());
        }

    }
}
