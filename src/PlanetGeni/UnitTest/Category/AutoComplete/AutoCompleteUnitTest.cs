using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Custom;
using DTO.Db;
using Manager.Jobs;
using Manager.ServiceController;
using Newtonsoft.Json;
using NUnit.Framework;
using Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Model;

namespace UnitTest.Category
{
    [TestFixture]
    [Category("AutoComplete")]
    public class AutoComplete
    {
        private static string Category = "AutoComplete";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.AutoComplete"];
        private StoredProcedureExtender spContext = new StoredProcedureExtender();
        private RedisCacheProviderExtender cache = new RedisCacheProviderExtender(true, AppSettings.RedisDatabaseId);
        private RedisCacheProvider autocompletecache = new RedisCacheProvider(AppSettings.RedisAutocompleteDatabaseId);
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        private UnitTestFixture setupFixture;
        public AutoComplete()
        {
            string[] createtables = new string[] { };
            setupFixture = new UnitTestFixture(spContext, createtables);
        }
        [TestFixtureSetUp]
        public void Init()
        {
            //return;
            setupFixture.BootStrapDb();


        }
        [SetUp]
        public void SetupTestData()
        {
            setupFixture.LoadDataTable(Category, rootFolderCategory);
            cache.FlushAllDatabase();
        }

        [Test]
        [TestCase("HKWU", "IWU",
            "GetWebUserIndexList",
                "UserId",
                new string[] { "CountryId", "Picture", "UserId" }, new string[] { "fre", "ba" }, 5, 4, 4, 1)]
        public void TestAutoComplete(string hashkey, string setkey, string procedureName,
            string hasKeyFieldName, string[] setExceptionProperty, string[] queryString, int firstCount,
            int secondCount, int thirdCount, int fourthCount)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            AutoCompleteIndexManager<WebUserIndexDTO> webUserIndex =
      new AutoCompleteIndexManager<WebUserIndexDTO>
      (
             hashkey,
             setkey,
            procedureName,
             new Dictionary<string, object>(),
             AppSettings.RedisAutocompleteDatabaseId,
            hasKeyFieldName,
             new PropertyInfo[] { typeof(WebUserIndexDTO).GetProperty("EmailId") },
            setExceptionProperty

      );

            webUserIndex.IndexAll();
            CheckResult(queryString[0], firstCount, secondCount, false);
            CheckResult(queryString[1], thirdCount, fourthCount, false);
            webUserIndex.AddIndexItem(AddNewUser());
            CheckResult(queryString[1], thirdCount + 1, fourthCount + 1, false);
            webUserIndex.UpdateIndexItem(AddNewUser(), UpdatedNewUser());
            CheckResult(queryString[0], firstCount + 1, secondCount + 1, true);
            CheckResult(queryString[1], thirdCount + 1, fourthCount + 1, false);

            int newCount = UnitUtility.ElmahErrorCount(spContext);

            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 60 * 2, "Should take less than  2 minutes");
        }

        private void CheckResult(string queryString, int firstCount, int secondCount, bool emailMatch)
        {
            var result = autocompletecache.AutoCompleteSearch(AppSettings.RedisHashIndexWebUser, AppSettings.RedisSetIndexWebUser, queryString, 0, 10);
            List<WebUserIndexDTO> webuserResult = result.Select(JsonConvert.DeserializeObject<WebUserIndexDTO>).ToList();
            Assert.AreEqual(firstCount, webuserResult.Count());
            if (!emailMatch)
            {
                Assert.AreEqual(firstCount, webuserResult.Count(f => f.NameFirst.ToLower().Contains(queryString) || f.NameLast.ToLower().Contains(queryString)));
            }



            result = autocompletecache.AutoCompleteSearch(AppSettings.RedisHashIndexWebUser, AppSettings.RedisSetIndexWebUser, queryString, 4, 10);
            webuserResult = result.Select(JsonConvert.DeserializeObject<WebUserIndexDTO>).ToList();
            Assert.AreEqual(secondCount, webuserResult.Count());
            if (!emailMatch)
            {
                Assert.AreEqual(secondCount, webuserResult.Count(f => f.NameFirst.ToLower().Contains(queryString) || f.NameLast.ToLower().Contains(queryString)));
            }


        }

        private WebUserIndexDTO AddNewUser()
        {

            WebUserIndexDTO newuser = new WebUserIndexDTO()
            {
                CountryId = "np",
                EmailId = "whatismyname@mail.com",
                NameFirst = "Bary",
                NameLast = "White",
                FullName = "Barry White",
                Picture = Guid.NewGuid().ToString(),
                UserId = 500

            };
            return newuser;

        }
        private WebUserIndexDTO UpdatedNewUser()
        {

            WebUserIndexDTO newuser = new WebUserIndexDTO()
            {
                CountryId = "np",
                EmailId = "freddyname@mail.com",
                NameFirst = "Bary",
                NameLast = "BarTender",
                FullName = "Barry BarTender",
                Picture = Guid.NewGuid().ToString(),
                UserId = 500

            };
            return newuser;

        }
    }
}
