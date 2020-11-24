namespace XeMart.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class UserProductReview : BaseDeletableModel<string>
    {
        public UserProductReview()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public byte Rating { get; set; }

        [Required]
        public string Content { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public string ProductId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
