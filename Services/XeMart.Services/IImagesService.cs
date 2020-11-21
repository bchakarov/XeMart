namespace XeMart.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IImagesService
    {
        public Task<string> UploadImage(IFormFile image, string path);
    }
}
