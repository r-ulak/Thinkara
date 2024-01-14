using System;
using System.Linq;
using System.Threading.Tasks;
using DTO.Custom; 
using PlanetGeni.ContentProviders.Core;
using PlanetGeni.HttpHelper;


namespace PlanetGeni.ContentProviders
{
    public class ImgurContentProvider : CollapsibleContentProvider
    {
        protected override Task<ContentProviderResult> GetCollapsibleContent(ContentProviderHttpRequest request)
        {
            string id = request.RequestUri.AbsoluteUri.Split('/').Last();

            return TaskAsyncHelper.FromResult(new ContentProviderResult()
            {
                Content = String.Format(@"<img src=""proxy?url=http://i.imgur.com/{0}.jpg"" />", id),
                Title = request.RequestUri.AbsoluteUri.ToString()
            });
        }

        public override bool IsValidContent(Uri uri)
        {
            return uri.AbsoluteUri.StartsWith("http://imgur.com/", StringComparison.OrdinalIgnoreCase);
        }
    }
}