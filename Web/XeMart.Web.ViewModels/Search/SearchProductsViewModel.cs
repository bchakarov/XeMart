namespace XeMart.Web.ViewModels.Search
{
    using System.Collections.Generic;
    using System.Linq;

    using XeMart.Web.ViewModels.Products;

    public class SearchProductsViewModel : PagingViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }

        public IEnumerable<string> SortingValues { get; set; }

        public string Sorting { get; set; }

        public string SearchTerm { get; set; }

        public int? MainCategoryId { get; set; }

        public IEnumerable<int> ItemsPerPageValues { get; set; }

        public override Dictionary<string, string> GetPageQuery(int pageNumber)
        {
            var baseDictionary = base.GetPageQuery(pageNumber);

            if (this.ItemsPerPage != this.ItemsPerPageValues.FirstOrDefault())
            {
                baseDictionary.Add("ItemsPerPage", this.ItemsPerPage.ToString());
            }

            if (this.Sorting.ToLower() != this.SortingValues.FirstOrDefault().ToLower())
            {
                baseDictionary.Add("Sorting", this.Sorting);
            }

            baseDictionary.Add("SearchTerm", this.SearchTerm);

            if (this.MainCategoryId != null)
            {
                baseDictionary.Add("MainCategoryId", this.MainCategoryId.ToString());
            }

            return baseDictionary;
        }
    }
}
