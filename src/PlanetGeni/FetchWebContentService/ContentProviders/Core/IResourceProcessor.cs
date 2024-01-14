using System.Threading.Tasks;
using DTO.Custom;

namespace PlanetGeni.ContentProviders.Core
{
    public interface IResourceProcessor
    {
        Task<ContentProviderResult> ExtractResource(string url);
    }
}
