using FlipShop.Core.Abstractions.Products;
using FlipShop.Infrastructure.Repositories.Products;
using Microsoft.AspNetCore.Mvc;

namespace FlipShop.WebApi.Components
{
	public class LatestProductsViewComponent : ViewComponent
	{
		private readonly IProductRepository _productService;

		public LatestProductsViewComponent(IProductRepository productService)
		{
			this._productService = productService;

        }

		public async Task<IViewComponentResult> InvokeAsync(int count)
		{
			return View(await _productService.GetTopProducts(count));
		}
	}
}
