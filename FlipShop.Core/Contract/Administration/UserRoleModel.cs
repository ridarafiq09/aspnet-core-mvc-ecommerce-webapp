using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Contract.Administration
{
    public class UserRoleModel
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public bool isSelected { get; set; }

    }
}
