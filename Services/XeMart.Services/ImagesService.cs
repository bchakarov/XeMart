namespace XeMart.Services
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class ImagesService : IImagesService
    {
        public async Task<string> UploadImage(IFormFile image, string path)
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            await image.CopyToAsync(stream);

            return stream.Name;
        }
    }
}
