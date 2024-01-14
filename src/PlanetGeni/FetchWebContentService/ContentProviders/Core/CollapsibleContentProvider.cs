using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DTO.Custom;
using Microsoft.Security.Application;
using PlanetGeni.HttpHelper;

namespace PlanetGeni.ContentProviders.Core
{
    public abstract class CollapsibleContentProvider : IContentProvider
    {
        public virtual Task<ContentProviderResult> GetContent(ContentProviderHttpRequest request)
        {
            return GetCollapsibleContent(request).Then(result =>
            {
                if (IsCollapsible && result != null)
                {
                    result.Content = String.Format(CultureInfo.InvariantCulture,
                                                      ContentFormat,
                                                      result.Content);
                }

                return result;
            });
        }

        protected virtual Regex ParameterExtractionRegex
        {
            get
            {
                return new Regex(@"(\d+)");

            }
        }

        protected virtual IList<string> ExtractParameters(Uri responseUri)
        {
            return ParameterExtractionRegex.Match(responseUri.AbsoluteUri)
                                .Groups
                                .Cast<Group>()
                                .Skip(1)
                                .Select(g => g.Value)
                                .Where(v => !String.IsNullOrEmpty(v)).ToList();

        }
        protected abstract Task<ContentProviderResult> GetCollapsibleContent(ContentProviderHttpRequest request);

        public virtual bool IsValidContent(Uri uri)
        {
            return false;
        }

        protected virtual bool IsCollapsible { get { return true; } }

        private const string ContentFormat = @"<div class=""content"">{0}</div>";
    }
}