using FlipShop.Core.Abstractions.Products;
using FlipShop.Core.Contract;
using FlipShop.Core.Contract.Cart;
using FlipShop.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Infrastructure.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {

        private readonly FlipShopContext flipShopContext;

        public ProductRepository(FlipShopContext flipShopContext)
        {
            this.flipShopContext = flipShopContext;

        }

        public async Task<List<Product>> GetTopProducts(int count)
        {
            return await flipShopContext.Products.Where(product => product.Product_Price < 100).Take(count).ToListAsync();
        }

        public IQueryable<Product> SearchProducts(string searchQuery, string sortOrder)
        {
            const string sortByName = "sortByName";
            const string priceLowerToHigher = "priceLowerToHigher";
            const string priceHigherToLower = "priceHigherToLower";
            var filteredItems = flipShopContext.Products.Where(product => product.Product_Name.Contains(searchQuery) || string.IsNullOrEmpty(searchQuery));
            switch (sortOrder)
            {
                case sortByName:
                    filteredItems = filteredItems.OrderByDescending(product => product.Product_Name);
                    break;
                case priceLowerToHigher:
                    filteredItems = filteredItems.OrderBy(product => product.Product_Price);
                    break;
                case priceHigherToLower:
                    filteredItems = filteredItems.OrderByDescending(product => product.Product_Price);
                    break;
                default:
                    filteredItems = filteredItems.OrderBy(product => product.Product_Price > 20);
                    break;
            }

            return filteredItems;

        }

        public async Task<IEnumerable<CartItemModel>> GetCartItems(List<CartItemCookie> cartItems)
        {
            var productsCart = new List<CartItemModel>();

            foreach (var productItem in cartItems)
            {
                var product = await flipShopContext.Products.FindAsync(productItem.product_id);
                if (product is null) continue;

                var item = new CartItemModel()
                {
                    Product_Description = product.Product_Description,
                    Product_Name = product.Product_Name,
                    Product_ID = product.Product_ID,
                    Product_ImagePath = product.Product_ImagePath,
                    Product_Price = product.Product_Price,
                    Product_quantity = productItem.quantity
                };


                productsCart.Add(item);
            }

            return productsCart;
        }

        public CartDetails CalculateBill(CartDetails cartDetails)
        {

            decimal SubTotal = 0;
            const decimal Tax = 0.5M;
            decimal ShippingFee = 3;

            cartDetails.CartItems.ToList().ForEach(cartItem =>
            {
                SubTotal += (cartItem.Product_quantity * cartItem.Product_Price);

            });

            cartDetails.SubTotal = SubTotal;
            cartDetails.ShippingFee = ShippingFee;
            cartDetails.Tax = Tax;

            switch (SubTotal)
            {
                case > 50:
                    cartDetails.ShippingFee = 0;
                    cartDetails.TotalAmount = Tax + SubTotal + ShippingFee;
                    return cartDetails;
                default:
                    cartDetails.TotalAmount = Tax + SubTotal + ShippingFee;
                    return cartDetails;
            }

        }
        public async Task<List<Category>> GetCategories()
        {
            return await flipShopContext.Categories.ToListAsync();
        }
        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            return flipShopContext.Products.Where(product => product.Category_Id.Equals(categoryId)).ToList();
            
        }

        
    }
}
