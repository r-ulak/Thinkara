using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Security.Application;

namespace PlanetX.Infrastructure
{

    public class TextTransform
    {


        static Regex urlPattern = new Regex(@"(?i)\b((?:[a-z][\w-]+:(?:/{1,3}|[a-z0-9%])|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’]))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string TransformAndExtractUrls(string message, out HashSet<string> extractedUrls)
        {
            var urls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            message = urlPattern.Replace(message, m =>
            {
                string url = HttpUtility.HtmlDecode(m.Value);
                if (!url.Contains("://"))
                {
                    url = "http://" + url;
                }

                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    return m.Value;
                }

                urls.Add(url);

                return String.Format(CultureInfo.InvariantCulture,
                                     "<a rel=\"nofollow external\" target=\"_blank\" href=\"{0}\" title=\"{1}\">{1}</a>",
                                     Encoder.HtmlAttributeEncode(url),
                                     m.Value);
            });

            extractedUrls = urls;
            return message;
        }

        public string Parse(string message)
        {
            return ConvertTextWithNewLines(message);
        }

        private string ConvertTextWithNewLines(string message)
        {
            // If the message contains new lines wrap all of it in a pre tag
            if (message.Contains('\n'))
            {
                return String.Format(@"
                            <div class=""collapsible_content"">
                                <h3 class=""collapsible_title"">Paste (click to show/hide)</h3>
                                <div class=""collapsible_box"">
                                    <pre class=""multiline"">{0}</pre>
                                </div>
                            </div>
                            ", message);
            }

            return message;
        }


    }
}
