namespace XeMart.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class Supplier : BaseDeletableModel<int>
    {
        public Supplier()
        {
            this.IsDefault = false;
        }

        [Required]
        public string Name { get; set; }

        public decimal PriceToHome { get; set; }

        public decimal PriceToOffice { get; set; }

        public bool IsDefault { get; set; }
    }
}
