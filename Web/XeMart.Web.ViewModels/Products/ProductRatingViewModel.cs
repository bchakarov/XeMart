namespace XeMart.Web.ViewModels.Products
{
    using System;

    public class ProductRatingViewModel
    {
        public double AverageRating { get; set; }

        public double AverageRatingRounded => Math.Round(this.AverageRating * 2, MidpointRounding.AwayFromZero) / 2;
    }
}
