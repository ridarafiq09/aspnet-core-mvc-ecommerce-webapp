using AutoMapper;
using FlipShop.Core.Contract;
using FlipShop.Core.Contract.Cart;
using FlipShop.Core.Contract.Orders;
using FlipShop.Core.Entities;

namespace FlipShop.WebApi.Common
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<ProductDTO, Product>();
            CreateMap<Product, ProductDTO>();
            CreateMap<OrderAddress, UserAddressDTO>();
            CreateMap<UserTransactionDetails, OrderTransactionDTO>();
            CreateMap<Order, OrderDetailsModel>();
            CreateMap<Order, OrderAddress>();
            CreateMap<CartDetails, Order>();
            CreateMap<Order, CartDetails>();
            CreateMap<UserAddressDTO, OrderAddress>();
            CreateMap<OrderTransactionDTO, UserTransactionDetails>();
            CreateMap<OrderDetailsModel, Order>();
            CreateMap<OrderInfo, Order>().ForMember(x => x.Payment_Type, opt => opt.Ignore());
            CreateMap<Order, OrderInfo>().ForMember(x => x.Payment_Type, opt => opt.Ignore());
        }
    }
}
