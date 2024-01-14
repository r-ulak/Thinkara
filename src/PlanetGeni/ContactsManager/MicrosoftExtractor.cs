using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.GData.Contacts;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.Contacts;
using System.Configuration;
using DAO.Models;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace ContactsManager
{
    public static class MicrosoftExtractor
    {
        public static List<WebUserContact> ExtractEmail(string accessToken)
        {
            try
            {
                string queryParm = String.Format("access_token={0}", HttpUtility.UrlEncode(accessToken));
                string url = "https://apis.live.net/v5.0/me/contacts?" + queryParm;
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = WebRequestMethods.Http.Get;
                List<WebUserContact> userContacts = new List<WebUserContact>();
                using (HttpWebResponse response =
                              (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        var json = JObject.Parse(rawJson);  //Turns your raw string into a key value lookup


                        int index = 0;
                        foreach (var item in json["data"])
                        {
                            WebUserContact contact = new WebUserContact();
                            contact.NameFirst = item["first_name"].ToString();
                            contact.NameLast = item["last_name"].ToString();
                            contact.FriendEmailId = item["emails"]["preferred"].ToString().ToLower();

                            userContacts.Add(contact);
                            index++;
                        }
                    }
                }
                return userContacts;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to Import microsoft Contacts");
                return null;
            }

        }

    }
}
