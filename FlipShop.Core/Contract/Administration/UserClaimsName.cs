using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Contract.Administration
{
    public class UserClaimsName
    {

        public UserClaimsName()
        {
            Claims = new();
        }

        public string UserId { get; set; }
        public List<UserClaimModel> Claims { get; set; }
    }
}
