namespace XeMart.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class UserMessage : BaseDeletableModel<string>
    {
        public UserMessage()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsRead = false;
        }

        [Required]
        [MaxLength(50)]
        public string Subject { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; }

        [Required]
        public string IP { get; set; }

        public bool IsRead { get; set; }
    }
}
