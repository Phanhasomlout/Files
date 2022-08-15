using Files.Sdk.Models.Imaging;
using System.Threading.Tasks;

namespace Files.Sdk.Services
{
    public interface IImagingService
    {
        Task<ImageModel?> GetImageModelFromDataAsync(byte[]? rawData);

        Task<ImageModel?> GetImageModelFromPathAsync(string filePath, uint thumbnailSize = 64u);
    }
}
