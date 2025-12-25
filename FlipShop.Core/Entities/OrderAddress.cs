using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Entities
{
    public class OrderAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public short Postal { get; set; }
        public string Country { get; set; }
        public int AddressId { get; set; }
        public virtual Order Order { get; set; }
    }
}
