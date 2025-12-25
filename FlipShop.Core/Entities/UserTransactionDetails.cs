using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Entities
{
    public class UserTransactionDetails
    {
        public string NameOnCard { get; set; }
        public short CVV { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public long CardNumber { get; set; }
        public int TransactionId { get; set; }
        public virtual Order Order { get; set; }
    }
}
