namespace XeMart.Web.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;

    public class ContactFormInputViewModel
    {
        [Required]
        [MinLength(5, ErrorMessage = "The subject must be at least 5 characters long.")]
        [MaxLength(50, ErrorMessage ="The subject can be maximum 50 characters long.")]
        public string Subject { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "The message must be at least 10 characters long.")]
        [MaxLength(2000, ErrorMessage = "The message can be maximum 2000 characters long.")]
        public string Message { get; set; }
    }
}
