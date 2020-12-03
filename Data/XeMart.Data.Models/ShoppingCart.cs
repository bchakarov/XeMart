namespace XeMart.Data.Models
{
    using System;
    using System.Collections.Generic;

    using XeMart.Data.Common.Models;

    public class ShoppingCart : BaseDeletableModel<string>
    {
        public ShoppingCart()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; }
    }
}
