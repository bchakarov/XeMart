namespace XeMart.Web.ViewModels.Categories
{
    using System.Collections.Generic;
    using System.Linq;

    using XeMart.Web.ViewModels.Products;

    public class SubcategoryProductsViewModel : PagingViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }

        public IEnumerable<string> SortingValues { get; set; }

        public string Sorting { get; set; }

        public IEnumerable<int> ItemsPerPageValues { get; set; }

        public override Dictionary<string, string> GetPageQuery(int pageNumber)
        {
            var baseDictionary = base.GetPageQuery(pageNumber);
            baseDictionary.Add("SubcategoryId", this.Id.ToString());

            if (this.ItemsPerPage != this.ItemsPerPageValues.FirstOrDefault())
            {
                baseDictionary.Add("ItemsPerPage", this.ItemsPerPage.ToString());
            }

            if (this.Sorting.ToLower() != this.SortingValues.FirstOrDefault().ToLower())
            {
                baseDictionary.Add("Sorting", this.Sorting.ToString());
            }

            return baseDictionary;
        }
    }
}
