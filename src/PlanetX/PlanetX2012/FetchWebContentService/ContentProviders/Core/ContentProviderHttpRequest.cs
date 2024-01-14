using System;
using System.Net;
using System.Threading.Tasks;
using PlanetX.Infrastructure;

namespace PlanetX.ContentProviders.Core
{
    public class ContentProviderHttpRequest
    {        
        public ContentProviderHttpRequest(Uri url)
        {
            RequestUri = url;
        }

        public Uri RequestUri { get; private set; }
    }
}