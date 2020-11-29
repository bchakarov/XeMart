namespace XeMart.Web.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class PagingViewModel
    {
        public int SubcategoryId { get; set; }

        public int PageNumber { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1;

        public int PreviousPageNumber => this.PageNumber - 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int NextPageNumber => this.PageNumber + 1;

        public int PagesCount => (int)Math.Ceiling((double)this.ItemsCount / this.ItemsPerPage);

        public int ItemsCount { get; set; }

        public int ItemsPerPage { get; set; }

        public IEnumerable<int> ItemsPerPageValues { get; set; }
    }
}
