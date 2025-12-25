using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Contract.Administration
{
    public class RoleUserModel
    {
        public string roleId { get; set; }
        public string roleName { get; set; }
        public bool isSelected { get; set; }
    }
}
