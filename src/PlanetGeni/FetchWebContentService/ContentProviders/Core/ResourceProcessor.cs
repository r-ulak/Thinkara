using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Threading.Tasks;
using DTO.Custom;
using PlanetGeni.HttpHelper;

namespace PlanetGeni.ContentProviders.Core
{
    public class ResourceProcessor : IResourceProcessor
    {
        private readonly IList<IContentProvider> _contentProviders;

        public ResourceProcessor()
        {
            _contentProviders = GetContentProviders();
        }

        public Task<ContentProviderResult> ExtractResource(string url)
        {
            Uri resultUrl;
            if (Uri.TryCreate(url, UriKind.Absolute, out resultUrl))
            {
                var request = new ContentProviderHttpRequest(resultUrl);
                return ExtractContent(request);
            }

            return TaskAsyncHelper.FromResult<ContentProviderResult>(null);
        }

        private Task<ContentProviderResult> ExtractContent(ContentProviderHttpRequest request)
        {
            var validProviders = _contentProviders.Where(c => c.IsValidContent(request.RequestUri))
                                                  .ToList();

            if (validProviders.Count == 0)
            {
                return TaskAsyncHelper.FromResult<ContentProviderResult>(new ContentProviderResult { Uri = request.RequestUri.ToString(), Processed = true });
            }

            var tasks = validProviders.Select(c => c.GetContent(request)).ToArray();

            var tcs = new TaskCompletionSource<ContentProviderResult>();

            Task.Factory.ContinueWhenAll(tasks, completedTasks =>
            {
                var faulted = completedTasks.FirstOrDefault(t => t.IsFaulted);
                if (faulted != null)
                {
                    tcs.SetResult(new ContentProviderResult { Uri = request.RequestUri.ToString(), Processed = true });
                    //tcs.SetException(faulted.Exception);
                }
                else if (completedTasks.Any(t => t.IsCanceled))
                {
                    tcs.SetResult(new ContentProviderResult { Uri = request.RequestUri.ToString(), Processed = true });
                    tcs.SetCanceled();
                }
                else
                {
                    ContentProviderResult result = new ContentProviderResult();
                    try
                    {
                        result = completedTasks.Select(t => t.Result).FirstOrDefault(content => content != null);
                        result.Uri = request.RequestUri.ToString();
                        result.Processed = true;
                        tcs.SetResult(result);
                    }
                    catch (Exception )
                    {
                        tcs.SetResult(new ContentProviderResult { Uri = request.RequestUri.ToString(), Processed = true });
                        tcs.SetCanceled();
                    }

                }
            });

            return tcs.Task;
        }


        private static IList<IContentProvider> GetContentProviders()
        {
            // Use MEF to locate the content providers in this assembly
            var compositionContainer = new CompositionContainer(new AssemblyCatalog(typeof(ResourceProcessor).Assembly));
            //compositionContainer.ComposeExportedValue(settings);
            return compositionContainer.GetExportedValues<IContentProvider>().ToList();
        }
    }
}