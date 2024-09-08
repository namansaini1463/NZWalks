using WebApiNZwalks.Models.Domain;

namespace WebApiNZwalks.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
