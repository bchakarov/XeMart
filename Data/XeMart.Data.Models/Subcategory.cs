namespace XeMart.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class Subcategory : BaseDeletableModel<int>
    {
        public Subcategory()
        {
            this.Products = new HashSet<Product>();
        }

        [Required]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int MainCategoryId { get; set; }

        public virtual MainCategory MainCategory { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
