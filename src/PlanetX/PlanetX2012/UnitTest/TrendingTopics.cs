using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using DAO.DAO;
using DAO.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using PlanetX.Infrastructure;


namespace UnitTest
{
    [TestFixture]
    public class TrendingTopicsUnitTest
    {
        [Test]
        [Repeat(1)]
        public void AddTopicsWithoutHasTags()
        {
            PlanetXContext db = new PlanetXContext();

            List<TopicTag> tags = (from q in db.TopicTags
                                   select q).ToList();
            tags.ForEach(x => db.TopicTags.Remove(x));
            db.SaveChanges();
            string hashTag1 = @"# is intended to be a simple, modern, general-purpose, object-oriented programming language.[6] Its development team is led by Anders Hejlsberg. The most recent version is C# 5.0, which was released on August 15, 2012.";
            string hashTag2 = @"The C# language is intended to be a simple, modern, general-purpose, object-oriented programming language.";
            string hashTag3 = @"Source code portability is very important, as is programmer portability, especially for those programmers already familiar with C and C++.";
            string hashTag4 = @"Although C# applications are intended to be economical with regard to memory and processing power requirements, the language was not intended to compete directly on performance and size with C or assembly language.";

            string url = ConfigurationManager.AppSettings["trendingTopicUrl"];
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag1);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag2);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag3);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag4);

            System.Threading.Thread.Sleep(5000);
            List<TrendingTopics> actualTopics = GetTrendingTopicWebRequest(url + "/GetTrendingTopics");

            List<TrendingTopics> expectedTopics = new List<TrendingTopics>();

            CollectionAssert.AreEqual(expectedTopics, actualTopics);

        }

        [Test]
        [Repeat(1)]
        public void AddTopicsWithHasTags()
        {
            PlanetXContext db = new PlanetXContext();

            List<TopicTag> tags = (from q in db.TopicTags
                                   select q).ToList();
            tags.ForEach(x => db.TopicTags.Remove(x));
            db.SaveChanges();

            string hashTag1 = @"""# is #intended to be a simple, modern, general-purpose, object-oriented programming language.[6] Its development team is led by Anders Hejlsberg. The most recent version is C# 5.0, which was released on August 15, 2012.""";
            string hashTag2 = @"""The C# #language is intended to be a #simple, modern, general-purpose, object-oriented programming language.""";
            string hashTag3 = @"""Source code #portability is very important, as is programmer portability, especially for those programmers already familiar with C and C++.""";
            string hashTag4 = @"""Although C# applications are intended to be economical with regard to memory and processing power requirements, the language was not intended to compete directly on performance and size with C or assembly language.""";

            string url = ConfigurationManager.AppSettings["trendingTopicUrl"];
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag1);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag2);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag3);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag4);

            System.Threading.Thread.Sleep(5000);
            List<TrendingTopics> actualTopics = GetTrendingTopicWebRequest(url + "/GetTrendingTopics");

            List<TrendingTopics> expectedTopics = new List<TrendingTopics>();
            expectedTopics.Add(new TrendingTopics() { Tag = "INTENDED", TagCount = 1 });
            expectedTopics.Add(new TrendingTopics() { Tag = "LANGUAGE", TagCount = 1 });
            expectedTopics.Add(new TrendingTopics() { Tag = "PORTABILITY", TagCount = 1 });
            expectedTopics.Add(new TrendingTopics() { Tag = "SIMPLE", TagCount = 1 });

            CollectionAssert.AreEquivalent(expectedTopics, actualTopics);

        }

        [Test]
        [Repeat(1)]
        public void AddTopicsWithHasTagsCount()
        {
            PlanetXContext db = new PlanetXContext();
            List<TopicTag> tags = (from q in db.TopicTags
                                   select q).ToList();
            tags.ForEach(x => db.TopicTags.Remove(x));
            db.SaveChanges();

            string hashTag1 = @"""# is #intended to be a simple, modern, general-purpose, #simple  object-oriented programming language.[6] Its development team is led by Anders Hejlsberg. The most recent version is C# 5.0, which was released on August 15, 2012.""";
            string hashTag2 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";
            string hashTag3 = @"""Source code #portability is very important, as is  #simple programmer portability, especially for those programmers already familiar with C and C++.""";
            string hashTag4 = @"""Although C# applications are intended to be economical  #simple with regard to memory and processing power requirements, the language was not intended to compete directly on performance and size with C or assembly language.""";
            string hashTag5 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";
            string hashTag6 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";
            string hashTag7 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";
            string hashTag8 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";
            string hashTag9 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";
            string hashTag10 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";
            string hashTag11 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";
            string hashTag12 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";
            string hashTag13 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";
            string hashTag14 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";
            string hashTag15 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";
            string hashTag16 = @"""The C# #language is intended to be a #simple, #simple #simple #simple #simple #simple #simple modern, general-purpose, object-oriented programming language.""";


            string url = ConfigurationManager.AppSettings["trendingTopicUrl"];
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag1);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag2);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag3);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag4);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag5);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag6);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag7);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag8);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag9);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag10);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag11);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag12);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag13);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag14);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag15);
            AddTopicWebRequest(url + "/AddNewTopicTag", hashTag16);
            System.Threading.Thread.Sleep(15000);

            List<TrendingTopics> actualTopics = GetTrendingTopicWebRequest(url + "/GetTrendingTopics");

            List<TrendingTopics> expectedTopics = new List<TrendingTopics>();
            expectedTopics.Add(new TrendingTopics() { Tag = "SIMPLE", TagCount = 94 });
            expectedTopics.Add(new TrendingTopics() { Tag = "INTENDED", TagCount = 1 });
            expectedTopics.Add(new TrendingTopics() { Tag = "LANGUAGE", TagCount = 13 });
            expectedTopics.Add(new TrendingTopics() { Tag = "PORTABILITY", TagCount = 1 });
            CollectionAssert.AreEquivalent(expectedTopics, actualTopics);

        }

        [Test]
        public void AddTopicUsingHttpClient()
        {

            string url = ConfigurationManager.AppSettings["trendingTopicUrl"];


            PlanetXContext db = new PlanetXContext();
            List<TopicTag> tags = (from q in db.TopicTags
                                   select q).ToList();
            tags.ForEach(x => db.TopicTags.Remove(x));
            db.SaveChanges();

            //string hashTag1 = @"""# is #intended to be a simple, modern, general-purpose, #simple  object-oriented programming language.[6] Its development team is led by Anders Hejlsberg. The most recent version is C# 5.0, which was released on August 15, 2012.""";


            HttpClient client = new HttpClient();
            HttpContent content = new StringContent("#Fly this is attes", Encoding.UTF8, "application/json");

            client.PostAsync(url + "/AddNewTopicTag", content);



        }

        public List<TrendingTopics> GetTrendingTopicWebRequest(string url)
        {

            Helper unitHelper = new Helper();
            string response = unitHelper.CallRestService(url);
            if (response == @"{""GetTrendingTopicsResult"":[]}")
                return new List<TrendingTopics>();

            List<TrendingTopics> topics = JsonConvert.DeserializeObject<List<TrendingTopics>>(response);
            return topics;

        }

        public void AddTopicWebRequest(string url, string contentMessage)
        {

            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.KeepAlive = false;
            req.Method = "POST";


            byte[] buffer = Encoding.ASCII.GetBytes(contentMessage);
            req.ContentLength = buffer.Length;
            req.ContentType = "application/json";
            Stream PostData = req.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

        }


    }
}
