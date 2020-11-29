namespace XeMart.Web.ViewModels.Categories
{
    using AutoMapper;

    using System.Collections.Generic;

    using XeMart.Web.ViewModels.Products;

    public class SubcategoryProductsViewModel : PagingViewModel
    {
        public string Name { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }

        [IgnoreMap]
        public IEnumerable<int> ItemsPerPageValues { get; set; }
    }
}
