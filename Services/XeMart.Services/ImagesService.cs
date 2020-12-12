namespace XeMart.Services
{
    using System.IO;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using Microsoft.AspNetCore.Http;

    public class ImagesService : IImagesService
    {
        private readonly Cloudinary cloudinaryUtility;
        private readonly BlobServiceClient blobServiceClient;

        public ImagesService(
            Cloudinary cloudinaryUtility,
            BlobServiceClient blobServiceClient)
        {
            this.cloudinaryUtility = cloudinaryUtility;
            this.blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadLocalImageAsync(IFormFile image, string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
            var fullPath = directoryPath + image.FileName;

            using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            await image.CopyToAsync(stream);

            return stream.Name;
        }

        public async Task<string> UploadCloudinaryImageAsync(IFormFile image, string folderName)
        {
            byte[] destinationData;

            using (var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                destinationData = ms.ToArray();
            }

            UploadResult uploadResult = null;

            using (var ms = new MemoryStream(destinationData))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = folderName,
                    File = new FileDescription(image.FileName, ms),
                };

                uploadResult = this.cloudinaryUtility.Upload(uploadParams);
            }

            return uploadResult?.SecureUrl.AbsoluteUri;
        }

        public async Task<string> UploadAzureBlobImageAsync(IFormFile image, string containerName)
        {
            var container = this.blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = container.GetBlobClient(image.FileName);

            byte[] destinationData;

            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            destinationData = ms.ToArray();
            ms.Position = 0;
            var uploadOptions = new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = image.ContentType } };
            await blobClient.UploadAsync(ms, uploadOptions);
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
