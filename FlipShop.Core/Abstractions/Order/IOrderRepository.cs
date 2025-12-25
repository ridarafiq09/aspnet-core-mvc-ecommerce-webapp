using FlipShop.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Abstractions.Order
{
    public interface IOrderRepository
    {
        void PlaceOrder(OrderDetailsModel order);
    }
}
