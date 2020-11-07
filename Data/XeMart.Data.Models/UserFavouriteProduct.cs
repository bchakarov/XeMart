namespace XeMart.Data.Models
{
    using System;

    using XeMart.Data.Common.Models;

    public class UserFavouriteProduct : BaseDeletableModel<string>
    {
        public UserFavouriteProduct()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public virtual ApplicationUser User { get; set; }

        public string UserId { get; set; }

        public virtual Product Product { get; set; }

        public string ProductId { get; set; }
    }
}
