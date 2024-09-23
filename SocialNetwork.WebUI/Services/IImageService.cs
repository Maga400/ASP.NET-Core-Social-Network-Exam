namespace SocialNetwork.WebUI.Services
{
    public interface IImageService
    {
        Task<string> SaveFile(IFormFile file);
    }
}
