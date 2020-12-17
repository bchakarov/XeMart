namespace XeMart.Web.ViewComponents
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.ML;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Products;
    using XeMart.Web.ViewModels.Recommender;

    public class ProductRecommenderViewComponent : ViewComponent
    {
        private readonly PredictionEnginePool<ProductInfo, ProductPrediction> predictionEnginePool;
        private readonly IProductsService productsService;

        public ProductRecommenderViewComponent(
            PredictionEnginePool<ProductInfo, ProductPrediction> predictionEnginePool,
            IProductsService productsService)
        {
            this.predictionEnginePool = predictionEnginePool;
            this.productsService = productsService;
        }

        public IViewComponentResult Invoke(string productId)
        {
            var allProducts = this.productsService.GetAll<ProductSidebarViewModel>().Where(x => x.Id != productId);

            var recommendedProducts = new List<ProductSidebarViewModel>();

            foreach (var product in allProducts)
            {
                var prediction = this.predictionEnginePool.Predict(new ProductInfo { ProductId = productId, CopurchasedProductId = product.Id });
                if (prediction.Score > 0.80)
                {
                    recommendedProducts.Add(product);
                }
            }

            return this.View(recommendedProducts);
        }
    }
}
