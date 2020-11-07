namespace XeMart.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class ProductReview : BaseDeletableModel<string>
    {
        public ProductReview()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int Rating { get; set; }

        public string Content { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public string ProductId { get; set; }
    }
}
