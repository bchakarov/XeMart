namespace XeMart.Web.ViewModels.Search
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Web.ViewModels.Categories;

    public class SearchInputViewModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Search term cannot be less than 2 characters long.")]
        public string SearchTerm { get; set; }

        public int MainCategoryId { get; set; }

        public IEnumerable<MainCategoryNameViewModel> MainCategories { get; set; }
    }
}
