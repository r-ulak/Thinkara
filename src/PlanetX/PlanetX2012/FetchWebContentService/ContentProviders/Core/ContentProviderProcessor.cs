using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAO.DAO.ComplexType;

namespace PlanetX.ContentProviders.Core
{
    public class ContentProviderProcessor
    {
        public List<ContentProviderResult> ProcessUrls(IEnumerable<string> links)
        {

            var resourceProcessor = new ResourceProcessor();

            var contentTasks = links.Select(resourceProcessor.ExtractResource).ToArray();
            List<ContentProviderResult> contentWeb = new List<ContentProviderResult>();
            Task.WaitAll(contentTasks);

            foreach (var task in contentTasks)
            {
                if (task.IsFaulted)
                {
                    Trace.TraceError(task.Exception.GetBaseException().Message);
                    continue;
                }

                if (task.Result == null || String.IsNullOrEmpty(task.Result.Content))
                {
                    contentWeb.Add(task.Result);
                    continue;
                }
                contentWeb.Add(task.Result);
            }



            return contentWeb;
        }
    }
}