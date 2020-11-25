namespace XeMart.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IImagesService
    {
        public Task<string> UploadLocalImageAsync(IFormFile image, string fullDirectoryPath);

        public Task<string> UploadCloudinaryImageAsync(IFormFile image, string folderName);
    }
}
