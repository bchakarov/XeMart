namespace XeMart.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class ProductReviewInputViewModel : IMapTo<UserProductReview>
    {
        [Range(1, 5)]
        public byte Rating { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The name must be at least 3 characters long.")]
        [MaxLength(50, ErrorMessage = "The name can be maximum 50 characters long.")]
        public string Name { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "The content must be at least 10 characters long.")]
        [MaxLength(1000, ErrorMessage = "The content can be maximum 1000 characters long.")]
        public string Content { get; set; }

        public string UserId { get; set; }

        public string ProductId { get; set; }
    }
}
