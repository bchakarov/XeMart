namespace XeMart.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class City : BaseModel<int>
    {
        public City()
        {
            this.Addresses = new HashSet<Address>();
        }

        [Required]
        public string Name { get; set; }

        public string ZIPCode { get; set; }

        [Required]
        public string Country { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
