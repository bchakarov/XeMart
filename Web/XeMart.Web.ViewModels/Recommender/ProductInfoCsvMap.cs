namespace XeMart.Web.ViewModels.Recommender
{
    using CsvHelper.Configuration;

    public class ProductInfoCsvMap : ClassMap<ProductInfoCsv>
    {
        public ProductInfoCsvMap()
        {
            this.Map(m => m.ProductId).Index(0).Name("productId");
            this.Map(m => m.CopurchasedProductId).Index(1).Name("copurchasedProductId");
        }
    }
}
