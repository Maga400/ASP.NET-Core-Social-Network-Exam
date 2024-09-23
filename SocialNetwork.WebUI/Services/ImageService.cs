
namespace SocialNetwork.WebUI.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var saveImage = Path.Combine(_environment.WebRootPath,"images",file.FileName);
            using (var img = new FileStream(saveImage, FileMode.OpenOrCreate)) 
            {
                await file.CopyToAsync(img);
            }
            return file.FileName.ToString();
        }
    }
}
