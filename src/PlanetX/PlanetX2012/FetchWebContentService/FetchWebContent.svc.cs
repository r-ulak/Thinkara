using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using PlanetX.ContentProviders.Core;
using Newtonsoft.Json;
using DAO.DAO.ComplexType;

namespace FetchWebContent
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class FetchWebContent : IFetchWebContent
    {
        public List<ContentProviderResult> GetUrlContent(string urls)
        {
            string[] stringSeparators = new string[] { "[stop]" };
            string[] result;
            result = urls.Split(stringSeparators, StringSplitOptions.None);

            ContentProviderProcessor urlProcessor = new ContentProviderProcessor();
            return (urlProcessor.ProcessUrls(result));


        }


    }
}
