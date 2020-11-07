namespace XeMart.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class Product : BaseDeletableModel<string>
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [ConcurrencyCheck]
        public decimal Price { get; set; }

        public virtual Subcategory Subcategory { get; set; }

        public int SubcategoryId { get; set; }
    }
}
