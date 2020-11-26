namespace XeMart.Web.Infrastructure.ValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.AspNetCore.Http;

    using SixLabors.ImageSharp;

    public class MultipleImageAttribute : ValidationAttribute
    {
        private readonly int maxFileSize;

        public MultipleImageAttribute(int maxFileSize = 2 * 1024 * 1024)
        {
            this.maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var images = value as IEnumerable<IFormFile>;

            if (images == null || images.Count() == 0)
            {
                return ValidationResult.Success;
            }

            foreach (var image in images)
            {
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
            }

            return ValidationResult.Success;
        }
    }
}
