using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;

namespace UnitTest
{
    public class PeopleSearch
    {
        public string f { get; set; }
        public string l { get; set; }
        public string e { get; set; }
        public string p { get; set; }
    }
    [TestFixture]
    public class AutoCompleteSearch
    {
        private string url = ConfigurationManager.AppSettings["searchServiceUrl"];
        [Test]
        public void SearchName()
        {
            Helper helperUnit = new Helper();
            string term = "/SearchPeople/K";

            string response = helperUnit.CallRestService(url + term);
            Console.WriteLine(response);
            string[] actual = JsonConvert.DeserializeObject<string[]>(response);

            Assert.AreEqual(actual.Length, 3);


        }
        [Test]
        public void SearchNameFull()
        {
            Helper helperUnit = new Helper();
            string term = "/SearchPeople/K Mar";

            string response = helperUnit.CallRestService(url + term);
            Console.WriteLine(response);
            string[] actual = JsonConvert.DeserializeObject<string[]>(response);

            Assert.AreEqual(actual.Length, 1);


        }

        [Test]
        public void SearchEmail()
        {
            Helper helperUnit = new Helper();
            string term = "/SearchPeople/Kmartin@abc.com";

            string response = helperUnit.CallRestService(url + term);
            Console.WriteLine(response);
            string[] actual = JsonConvert.DeserializeObject<string[]>(response);

            Assert.AreEqual(actual.Length, 1);


        }

        [Test]
        public void SearchEmailNoResult()
        {
            Helper helperUnit = new Helper();
            string term = "/SearchPeople/Kmartin@abc.coms";

            string response = helperUnit.CallRestService(url + term);
            Console.WriteLine(response);
            string[] actual = JsonConvert.DeserializeObject<string[]>(response);

            Assert.AreEqual(actual, null);


        }

    }
}
