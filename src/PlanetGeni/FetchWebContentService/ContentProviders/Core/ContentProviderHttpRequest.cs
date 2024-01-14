using System;
using System.Net;
using System.Threading.Tasks;
using PlanetGeni.HttpHelper;

namespace PlanetGeni.ContentProviders.Core
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