namespace XeMart.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class ProductImage : BaseDeletableModel<string>
    {
        public ProductImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string ImageUrl { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public string ProductId { get; set; }
    }
}
