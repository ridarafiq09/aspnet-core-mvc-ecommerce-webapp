using FlipShop.Core.Contract.Cart;
using FlipShop.Core.Entities;

namespace FlipShop.Core.Abstractions.Products
{
    public interface IProductRepository
	{
		public IQueryable<Product> SearchProducts(string searchQuery, string sortOrder);
        Task<List<Product>> GetTopProducts(int count);
		Task<IEnumerable<CartItemModel>> GetCartItems(List<CartItemCookie> cartItems);
		CartDetails CalculateBill(CartDetails cartDetails);
        Task<List<Category>> GetCategories();
        IEnumerable<Product> GetProductsByCategory(int categoryId);
   

    }
}