namespace XeMart.Web.ViewModels.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using SixLabors.ImageSharp;

    public class ImageAttribute : ValidationAttribute
    {
        private readonly int maxFileSize;

        public ImageAttribute(int maxFileSize)
        {
            this.maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var image = value as IFormFile;

            if (image == null)
            {
                return ValidationResult.Success;
            }

            var format = Image.DetectFormat(image.OpenReadStream());

            if (format == null ||
                (format.Name != "JPEG" &&
                format.Name != "JPG" &&
                format.Name != "PNG"))
            {
                return new ValidationResult("Only .jpeg, .jpg and .png file formats are allowed.");
            }

            if (image.Length > this.maxFileSize)
            {
                return new ValidationResult($"Allowed maximum size is {this.maxFileSize} kbs.");
            }

            return ValidationResult.Success;
        }
    }
}
