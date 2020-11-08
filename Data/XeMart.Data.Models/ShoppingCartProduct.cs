namespace XeMart.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class ShoppingCartProduct : BaseDeletableModel<string>
    {
        public ShoppingCartProduct()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public virtual ShoppingCart ShoppingCart { get; set; }

        [Required]
        public string ShoppingCartId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public string ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
