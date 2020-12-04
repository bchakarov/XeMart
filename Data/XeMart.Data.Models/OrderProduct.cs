namespace XeMart.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Common.Models;

    public class OrderProduct : BaseModel<int>
    {
        [Required]
        public string OrderId { get; set; }

        public virtual Order Order { get; set; }

        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
