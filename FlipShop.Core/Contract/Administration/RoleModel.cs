using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Contract.Administration
{
    public class RoleModel
    {
        public RoleModel()
        {
            Users = new();
        }

        public string roleId { get; set; }

        [Required, StringLength(3)]
        public string roleName { get; set; }
        public List<string> Users { get; set; }

    }
}
