namespace XeMart.Web.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class PagingViewModel : BasePagingViewModel
    {
        public int SubcategoryId { get; set; }

        public IEnumerable<int> ItemsPerPageValues { get; set; }

        public string Sorting { get; set; }

        public IEnumerable<string> SortingValues { get; set; }
    }
}
