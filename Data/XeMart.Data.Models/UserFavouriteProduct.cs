namespace XeMart.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class UserFavouriteProduct : BaseModel<string>
    {
        public UserFavouriteProduct()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
