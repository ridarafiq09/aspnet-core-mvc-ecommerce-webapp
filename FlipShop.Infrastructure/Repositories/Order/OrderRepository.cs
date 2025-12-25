using AutoMapper;
using FlipShop.Core.Abstractions.Order;
using FlipShop.Core.Contract;
using FlipShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Infrastructure.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly FlipShopContext _flipShopContext;
        private readonly IMapper mapper;

        public OrderRepository(FlipShopContext flipShopContext,IMapper mapper)
        {
            _flipShopContext = flipShopContext;
            this.mapper = mapper;
        }

        public void PlaceOrder(OrderDetailsModel orderInfo)
        {
            Order order = new();
            
            order= mapper.Map<Order>(orderInfo.customerOrderProducts);
            order.OrderAddress = mapper.Map<OrderAddress>(orderInfo.address);
            order.UserTransactionDetails = mapper.Map<UserTransactionDetails>(orderInfo.transactionDetails);
            
            order.OrderStatus = 0;
            order.Order_Date = DateTime.Now;
            order.CustomerID = orderInfo.CustomerID;
     

            switch (orderInfo.orderdetails.Payment_Type)
            {
                case PaymentType.CreditCard:
                    order.Payment_Type= (int)PaymentType.CreditCard;
                    break;
                case PaymentType.DebitCard:
                    order.Payment_Type = (int)PaymentType.DebitCard;
                    break;
                default:
                    throw new Exception();

            }
            var orderedProducts = orderInfo.customerOrderProducts.CartItems.Select(product => new OrderedProductDetails
            {
                ProductID = product.Product_ID,
                Unit_Price = product.Product_Price,
                Quantity = product.Product_quantity
            }).ToList();

            order.OrderDetails = orderedProducts;

            _flipShopContext.Orders.Add(order);
           
            _flipShopContext.SaveChanges();
        }

       
    }
}
