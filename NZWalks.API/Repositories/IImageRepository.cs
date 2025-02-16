using NZWalks.API.Models.Domain;

namespace NZWalks.API.Services
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
