namespace XeMart.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using XeMart.Data.Common.Models;

    public class Partner : BaseDeletableModel<int>
    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string CompanyUrl { get; set; }

        [Required]
        [ForeignKey(nameof(Manager))]
        public string ManagerId { get; set; }

        public virtual ApplicationUser Manager { get; set; }

        public bool IsApproved { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public string LogoUrl { get; set; }
    }
}
