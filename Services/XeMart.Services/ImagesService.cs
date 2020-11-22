namespace XeMart.Services
{
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using Microsoft.AspNetCore.Http;

    public class ImagesService : IImagesService
    {
        private readonly Cloudinary cloudinaryUtility;

        public ImagesService(Cloudinary cloudinaryUtility)
        {
            this.cloudinaryUtility = cloudinaryUtility;
        }

        public async Task<string> UploadLocalImageAsync(IFormFile image, string path)
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
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
    }
}
