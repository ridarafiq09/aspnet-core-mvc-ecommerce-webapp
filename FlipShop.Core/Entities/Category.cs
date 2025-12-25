using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Entities
{
    [Table("tbl_category")]
    public class Category
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int Category_ID { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Category_Title { get; set; }
        [Column(TypeName = "nvarchar(400)")]
        public string Category_Description { get; set; }

        [ForeignKey("Category_Id")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
