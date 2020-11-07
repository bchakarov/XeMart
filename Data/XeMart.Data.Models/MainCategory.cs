namespace XeMart.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class MainCategory : BaseDeletableModel<int>
    {
        public MainCategory()
        {
            this.Subcategories = new HashSet<Subcategory>();
        }

        [Required]
        public string Name { get; set; }

        public string FontAwesomeIcon { get; set; }

        public string ImageUrl { get; set; }

        public virtual ICollection<Subcategory> Subcategories { get; set; }
    }
}
