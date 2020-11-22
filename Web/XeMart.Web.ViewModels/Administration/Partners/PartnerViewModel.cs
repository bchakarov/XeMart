namespace XeMart.Web.ViewModels.Administration.Partners
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class PartnerViewModel : IMapFrom<Partner>
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string CompanyUrl { get; set; }

        public string ManagerEmail { get; set; }
    }
}
