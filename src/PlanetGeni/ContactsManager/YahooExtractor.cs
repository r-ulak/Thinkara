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
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace ContactsManager
{
    public static class YahooExtractor
    {
        public static List<WebUserContact> ExtractEmail(string yahooGuid, string oAuthToken, string oathTokenSecret)
        {
            OAuthBase oauth = new OAuthBase();

            string consumerKey = ConfigurationManager.AppSettings["Yahoo:ClientId"];
            string consumerSecret = ConfigurationManager.AppSettings["Yahoo:ClientSecret"];


            Uri uri = new Uri("https://social.yahooapis.com/v1/user/" + yahooGuid + "/contacts;out=name,email;email.present=1?format=json");
            string nonce = oauth.GenerateNonce();
            string timeStamp = oauth.GenerateTimeStamp();
            string normalizedUrl;
            string normalizedRequestParameters;
            string sig = oauth.GenerateSignature(uri, consumerKey, consumerSecret, oAuthToken, oathTokenSecret, "GET", timeStamp, nonce, OAuthBase.SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedRequestParameters);
            List<WebUserContact> userContacts = new List<WebUserContact>();
            StringBuilder sbGetContacts = new StringBuilder(uri.ToString());
            try
            {
                string returnStr = string.Empty;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sbGetContacts.ToString());
                req.Method = "GET";
                string authHeader = "Authorization: OAuth " +
                "realm=\"yahooapis.com\"" +
                ",oauth_consumer_key=\"" + consumerKey + "\"" +
                ",oauth_nonce=\"" + nonce + "\"" +
                ",oauth_signature_method=\"HMAC-SHA1\"" +
                ",oauth_timestamp=\"" + timeStamp + "\"" +
                ",oauth_token=\"" + oAuthToken + "\"" +
                ",oauth_version=\"1.0\"" +
                ",oauth_signature=\"" + HttpUtility.UrlEncode(sig) + "\"";
                req.Headers.Add(authHeader);
                req.ContentType = "application/json";

                using (HttpWebResponse response =
               (HttpWebResponse)req.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {


                        var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        var json = JObject.Parse(rawJson);  //Turns your raw string into a key value lookup


                        int index = 0;
                        foreach (var item in json["contacts"]["contact"])
                        {
                            WebUserContact contact = new WebUserContact();
                            foreach (var field in item["fields"])
                            {
                                if (field["type"].ToString() == "name")
                                {
                                    contact.NameFirst = field["value"]["givenName"].ToString();
                                    contact.NameLast = field["value"]["familyName"].ToString();

                                }
                                else if (field["type"].ToString() == "email")
                                {
                                    contact.FriendEmailId = field["value"].ToString().ToLower();
                                }
                            }
                            userContacts.Add(contact);
                            index++;
                        }
                    }
                }
                return userContacts;
            }

            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to Import yahoo Contacts");
                return null;
            }

        }
    }

}
