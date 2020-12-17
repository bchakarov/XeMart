namespace XeMart.Web.ViewModels.Recommender
{
    using Microsoft.ML.Data;

    public class ProductInfo
    {
        [LoadColumn(0)]
        public string ProductId { get; set; }

        [LoadColumn(1)]
        public string CopurchasedProductId { get; set; }

        [LoadColumn(2)]
        public float Label { get; set; }
    }
}
