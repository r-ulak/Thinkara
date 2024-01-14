using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace UnitTest
{
    public class Helper
    {
        public string CallRestService(string url)
        {

            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.KeepAlive = false;
            req.Method = "GET";



            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

            Encoding enc = System.Text.Encoding.GetEncoding(1252);
            StreamReader loResponseStream =
            new StreamReader(resp.GetResponseStream(), enc);

            string response = loResponseStream.ReadToEnd();

            loResponseStream.Close();
            resp.Close();
            return response;

        }

    }
}
