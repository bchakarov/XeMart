namespace XeMart.Web.ViewModels.Administration.Products
{
    using System.Collections.Generic;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class EditProductViewModel : CreateProductInputViewModel, IMapFrom<Product>
    {
        public string Id { get; set; }

        public IEnumerable<ProductImage> Images { get; set; }
    }
}
