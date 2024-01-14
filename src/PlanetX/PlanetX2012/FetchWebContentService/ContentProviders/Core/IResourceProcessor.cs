using System.Threading.Tasks;
using DAO.DAO.ComplexType;

namespace PlanetX.ContentProviders.Core
{
    public interface IResourceProcessor
    {
        Task<ContentProviderResult> ExtractResource(string url);
    }
}
