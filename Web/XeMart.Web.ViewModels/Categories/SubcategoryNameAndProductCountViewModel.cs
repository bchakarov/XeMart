﻿namespace XeMart.Web.ViewModels.Categories
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class SubcategoryNameAndProductCountViewModel : IMapFrom<Subcategory>
    {
        public string Name { get; set; }

        public int ProductsCount { get; set; }
    }
}
