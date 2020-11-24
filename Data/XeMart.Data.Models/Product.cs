namespace XeMart.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class Product : BaseDeletableModel<string>
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Reviews = new HashSet<UserProductReview>();
            this.Images = new HashSet<ProductImage>();
            this.FavouriteProducts = new HashSet<UserFavouriteProduct>();
            this.ShoppingCartProducts = new HashSet<ShoppingCartProduct>();
        }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [ConcurrencyCheck]
        public decimal Price { get; set; }

        public int SubcategoryId { get; set; }

        public virtual Subcategory Subcategory { get; set; }

        public virtual ICollection<UserProductReview> Reviews { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; }

        public virtual ICollection<UserFavouriteProduct> FavouriteProducts { get; set; }

        public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; }
    }
}
