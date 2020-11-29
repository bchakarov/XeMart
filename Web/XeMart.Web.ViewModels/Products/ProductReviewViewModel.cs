namespace XeMart.Web.ViewModels.Products
{
    using System.Globalization;

    using AutoMapper;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class ProductReviewViewModel : IMapFrom<UserProductReview>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public byte Rating { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string CreatedOn { get; set; }

        public string UserEmail { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserProductReview, ProductReviewViewModel>()
            .ForMember(
                source => source.CreatedOn,
                destination => destination.MapFrom(member => member.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
