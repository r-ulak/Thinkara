using System.Threading.Tasks;

namespace PlanetX.ContentProviders.Core
{
    public interface IResourceProcessor
    {
        Task<ContentProviderResult> ExtractResource(string url);
    }
}
