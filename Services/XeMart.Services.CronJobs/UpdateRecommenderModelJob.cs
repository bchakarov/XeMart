namespace XeMart.Services.CronJobs
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using CsvHelper;

    using Microsoft.ML;
    using Microsoft.ML.Trainers;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Web.ViewModels.Recommender;

    public class UpdateRecommenderModelJob
    {
        private readonly IRepository<OrderProduct> orderProductsRepository;

        public UpdateRecommenderModelJob(IRepository<OrderProduct> orderProductsRepository)
        {
            this.orderProductsRepository = orderProductsRepository;
        }

        public void Work(string webRootPath)
        {
            var groupedOrders = this.orderProductsRepository.AllAsNoTracking().ToList()
                .GroupBy(x => x.OrderId)
                .Select(x => x.ToList())
                .ToList();

            var mainProductWithCopurchasedProducts = new List<ProductInfoCsv>();
            foreach (var orderProducts in groupedOrders.Where(x => x.Count > 1).Select(x => x.OrderByDescending(p => p.Price)))
            {
                var mainProductId = orderProducts.FirstOrDefault().ProductId;
                var copurchasedProductsIds = orderProducts.Skip(1).Select(x => x.ProductId).ToList();
                foreach (var copurchasedProductId in copurchasedProductsIds)
                {
                    mainProductWithCopurchasedProducts.Add(new ProductInfoCsv { ProductId = mainProductId, CopurchasedProductId = copurchasedProductId });
                }
            }

            var recommenderDirectoryPath = $"{webRootPath}\\Recommender";
            var modelPath = recommenderDirectoryPath + "\\model.zip";
            var trainingDataPath = $"{webRootPath}\\Recommender\\data.csv";

            if (!Directory.Exists(recommenderDirectoryPath))
            {
                Directory.CreateDirectory(recommenderDirectoryPath);
                using var fileStream = new FileStream(modelPath, FileMode.CreateNew);
            }

            using (var writer = new StreamWriter(trainingDataPath))
            {
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(mainProductWithCopurchasedProducts);
            }

            var context = new MLContext();

            // Load the dataset in memory
            var trainData = context.Data.LoadFromTextFile<ProductInfo>(
                trainingDataPath,
                hasHeader: true,
                separatorChar: ',');

            // Prepare matrix factorization options
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "ProductIdEncoded",
                MatrixRowIndexColumnName = "CopurchasedProductIdEncoded",
                LabelColumnName = "Label",
                LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossOneClass,
                NumberOfIterations = 100,
                Alpha = 0.01,
                Lambda = 0.025,
                C = 0.00001,
            };

            // Set up a training pipeline
            // Step 1: map ProductID and CombinedProductID to keys
            var pipeline = context.Transforms.Conversion.MapValueToKey(
                    inputColumnName: "ProductId",
                    outputColumnName: "ProductIdEncoded")
                .Append(context.Transforms.Conversion.MapValueToKey(
                    inputColumnName: "CopurchasedProductId",
                    outputColumnName: "CopurchasedProductIdEncoded"))

                // Step 2: find recommendations using matrix factorization
                .Append(context.Recommendation().Trainers.MatrixFactorization(options));

            // Train the model
            ITransformer model = pipeline.Fit(trainData);

            // Save the trained model
            context.Model.Save(model, trainData.Schema, modelPath);
        }
    }
}
