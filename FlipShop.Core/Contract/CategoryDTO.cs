using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Contract
{

    public class CategoryDTO
    {
        public int Category_ID { get; set; }
        public string Category_Title { get; set; }
        public string Category_Description { get; set; }
        public virtual ICollection<ProductDTO> Products { get; set; }
    }
}
