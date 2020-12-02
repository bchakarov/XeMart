namespace XeMart.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using XeMart.Data.Common.Models;

    public class ShoppingCart : BaseDeletableModel<string>
    {
        public ShoppingCart()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; }
    }
}
