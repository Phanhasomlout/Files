using Files.Sdk.Models;
using System.Threading.Tasks;

namespace Files.Sdk.Services
{
    public interface IVolumeInfoFactory
    {
        Task<VolumeInfo> BuildVolumeInfo(string driveName);
    }
}
