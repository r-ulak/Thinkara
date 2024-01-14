using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Db;
using Manager.ServiceController;
using Newtonsoft.Json;
using NUnit.Framework;
using Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Model;

namespace UnitTest.Category
{
    [TestFixture]
    [Category("Ads")]
    public class AdsUnitTest
    {
        private static string Category = "Ads";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.ads"];
        private StoredProcedureExtender spContext = new StoredProcedureExtender();
        private RedisCacheProviderExtender cache = new RedisCacheProviderExtender(true, AppSettings.RedisDatabaseId);
        private IRedisCacheProvider redisCache = new RedisCacheProvider(AppSettings.RedisDatabaseId);
        private IPartyDTORepository partyRepo = new PartyDTORepository();
        private IAdvertisementDetailsDTORepository adsRepo = new AdvertisementDetailsDTORepository();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        private IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();
        private ICountryCodeRepository countryRepo = new CountryCodeRepository();
        private UnitTestFixture setupFixture;
        public AdsUnitTest()
        {
            string[] createtables = new string[] { "CountryTax", Category, "PoliticalParty" };
            setupFixture = new UnitTestFixture(spContext, createtables);
        }
        [TestFixtureSetUp]
        public void Init()
        {
            //return;

            setupFixture.BootStrapDb();
            setupFixture.LoadDataTable();


        }
        [SetUp]
        public void SetupTestData()
        {
            string dataLoadsqlpath = @"\Sql\DataLoad" + Category + ".sql";

            StringBuilder boostrapSql = new StringBuilder();
            boostrapSql.Append(File.ReadAllText(rootFolder + rootFolderCategory
                + dataLoadsqlpath));

            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            boostrapSql.Replace("{0}", dataPath.Replace(@"\", @"\\"));
            spContext.ExecuteSql(boostrapSql.ToString());
            cache.FlushAllDatabase();


        }

        #region Buy Ads
        [Category("Ads")]
        [Test]
        [TestCase(1, 1, 5, new int[] { 1, 2, 3, 4 }, 189760
            , new int[] { 0, 5, 1, 2, 3, 4, 6 },
                 "2030/01/01",
                "https://www.youtube.com/watch?v=U1jwWwJ-Mxc https://i.ytimg.com/vi/U1jwWwJ-Mxc/hqdefault.jpg <A HREF=\"http://trusted.org/search.cgi?criteria=<SCRIPT SRC='http://evil.org/badkama.js'></SCRIPT>\"> Go to trusted.org</A>",
              "<div class=\"content\"><object ><param name=\"WMode\" value=\"transparent\"></param><param name=\"movie\" value=\"https://www.youtube.com/v/U1jwWwJ-Mxcfs=1\"></param><param name=\"allowFullScreen\" value=\"true\"></param><param name=\"allowScriptAccess\" value=\"always\"></param><embed src=\"https://www.youtube.com/v/U1jwWwJ-Mxc?fs=1\" wmode=\"transparent\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" allowscriptaccess=\"always\" ></embed></object></div><br /><div class=\"content\"><a rel=\"nofollow external\" target=\"_blank\" href=\"https://i.ytimg.com/vi/U1jwWwJ-Mxc/hqdefault.jpg\"><img src=\"https://i.ytimg.com/vi/U1jwWwJ-Mxc/hqdefault.jpg\" /></a></div><br />&lt;A HREF=&quot;http://trusted.org/search.cgi?criteria=&lt;SCRIPT SRC=&#39;http://evil.org/badkama.js&#39;&gt;&lt;/SCRIPT&gt;&quot;&gt; Go to trusted.org&lt;/A&gt;",
                 "2030/01/01",
                 "Title Sample",
                  189760 * .1, "Pass", ""
            )]

        [TestCase(1, 1, 5, new int[] { 1, 4, 3, 2 },
              54720,
            new int[] { 0, 3, 1, 2, 5, 4, 6 },
            "2030/01/01",
            "My Favorite Test", "My Favorite Test",
            "2030/01/01",
            "My Favorite Test", 54720 * .1, "Pass", "")]

        [TestCase(1, 1, 2, new int[] { 1, 4, 3, 2 },
                143640,
            new int[0],
            "2030/01/03",
            "My Favorite Test", "My Favorite Test",
            "2030/01/01",
            "My Favorite Test", 143640 * .1, "Pass", "")]


        [TestCase(1, 1, 1, new int[] { 1, 4 },
                 19380,
            new int[0],
            "2030/01/03",
            "My Favorite Test", "My Favorite Test",
            "2030/01/01",
            "My Favorite Test", 19380 * .1, "Pass", "")]

        [TestCase(1, 1, 2, new int[] { 1, 4 },
                 180880,
            new int[0],
            "2030/01/04",
            "My Favorite Test", "My Favorite Test",
            "2030/01/01",
            "My Favorite Test", 180880 * .1, "Pass", "")]

        [TestCase(1, 1, 4, new int[] { 1, 4 },
                   38760,
            new int[0],
            "2030/01/03",
            "My Favorite Test", "My Favorite Test",
            "2030/01/01",
            "My Favorite Test", 38760 * .1, "Pass", "")]

        [TestCase(1, 1, 3, new int[] { 1, 4 },
                  96900,
            new int[0],
            "2030/01/03",
            "My Favorite Test", "My Favorite Test",
            "2030/01/01",
            "My Favorite Test", 96900 * .1, "Pass", "")]

        [TestCase(1, 1, 5, new int[] { 1 },
                1980,
                new int[] { 0, 3 },
                "2030/01/01",
                "My Favorite Test", "My Favorite Test",
                "2030/01/01",
                "Title", 1980 * .1, "Pass", "")]

        [TestCase(1, 1, 5, new int[] { 1 },
            0,
            new int[] { 0, 3 },
            "2020/01/01",
          "Message.", "",
            "2030/01/01",
            "Iff", 0, "Fail", "Advertisement StartDate cannot be greater than EndDate")]

        [TestCase(1, 1, 5, new int[] { 1 },
            0,
            new int[] { 0, 3 },
            "2020/01/01",
          "Message", "",
            "2010/01/01",
            "Iff", 0, "Fail", "Advertisement StartDate cannot be in past")]

        [TestCase(1, 1, 5, new int[0],
            0,
            new int[] { 0, 3 },
            "2060/01/01",
          ".Message", "",
            "2045/01/01",
            "Ifff", 0, "Fail", "Ads Type is Required")]
        [TestCase(1, 100, 5, new int[] { 1 },
            0,
            new int[] { 0, 3 },
            "2060/01/01",
          "Message.", "",
            "2040/01/01",
            "Iii", 0, "Fail", "Ads Time is not in 24hr range")]

        [TestCase(1, 1, 5, new int[] { 1 },
             0,
             new int[0],
             "2060/01/01",
           "Message.", "",
             "2040/01/01",
             "Iii", 0, "Fail", "Ads Days was must be selected")]
        [TestCase(1, 1, 8, new int[] { 1 },
             0,
             new int[0],
             "2060/01/01",
           "Message.", "",
             "2040/01/01",
             "Iii", 0, "Fail", "Ads Frequency was Invalid")]
        [TestCase(1, 1, 5, new int[] { 1, 4, 3, 2 },
             1,
             new int[] { 0, 3, 1, 2, 5, 4, 6 },
             "2030/01/01",
             "My Favorite Test", "My Favorite Test",
             "2030/01/01",
             "My Favorite Test", 12, "Fail", "cost calculated does not add up")]

        [TestCase(1, 1, 5, new int[] { 1, 4, 3, 2 },
            1,
            new int[] { 0, 3, 1, 2, 5, 4, 6 },
            "today",
            "My Favorite Test", "My Favorite Test",
            "today",
            "My Favorite Test", 12, "Fail", "cost calculated does not add up")]

        [TestCase(1, 1, 5, new int[] { 1 },
            0,
            new int[] { 0, 3 },
            "2030/01/01",
            "I am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 Char.I am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 Char.I am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 Char.I am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 Char.I am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 Char.I am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 Char.I am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 Char.I am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 Char.I am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 Char.I am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 CharI am Longer than 1000 Char.", "",
            "2030/01/01",
            "Char.", 0, "Fail", "Advertisement Message length must be between 5 and 1000")]

        public void BuyAds(int userId, int adTime, sbyte adsFrequencyTypeId, int[] adsTypeList,
             decimal cost, int[] days, string endDate, string message, string previewMsg,
             string startDate, string title, decimal tax, string expectedResult, string failMsg
             )
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            AdvertisementPostDTO adsDetail = new AdvertisementPostDTO
            {
                UserId = userId,
                AdsFrequencyTypeId = adsFrequencyTypeId,
                AdsTypeList = adsTypeList,
                AdTime = adTime,
                Cost = cost,
                Days = days,
                Message = message,
                PreviewMsg = previewMsg,
                TotalCost = cost + tax,
                CalculatedTax = tax
            };


            if (endDate == "today")
            {
                adsDetail.EndDate = DateTime.UtcNow.Date;
            }
            else
            {
                adsDetail.EndDate = DateTime.ParseExact(endDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime().Date;
            }


            if (startDate == "today")
            {
                adsDetail.StartDate = DateTime.UtcNow.Date;
            }
            else
            {
                adsDetail.StartDate = DateTime.ParseExact(startDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime().Date;
            }

            AdsManager manager = new AdsManager();

            UserBankAccount oldbankac = bankRepo.GetUserBankDetails(adsDetail.UserId); manager.ProcessAds(adsDetail);
            if (expectedResult == "Pass")
            {
                CheckAdsResult(adsDetail, oldbankac);
            }
            else
            {
                CheckAdsResultFail(adsDetail, oldbankac, failMsg);

            }

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;

        }

        public void CheckAdsResult(AdvertisementPostDTO adsDetail, UserBankAccount oldbankac)
        {
            setupFixture.CheckUserBankAccount(oldbankac, -adsDetail.TotalCost, 0, 0);
            setupFixture.CheckUserNotification(new int[] { adsDetail.UserId }, new string[0],
                 AppSettings.AdsSuccessNotificationId, false, 1, 1);
            CheckAds(adsDetail, 1);

            adsDetail.CountryId = webRepo.GetCountryId(adsDetail.UserId);

            setupFixture.CheckCapitalTransactionLog(adsDetail.UserId,
                countryRepo.GetCountryCode(adsDetail.CountryId).CountryUserId,
                adsDetail.Cost,
                adsDetail.CalculatedTax,
                 AppSettings.AdsFundType, 1);
        }
        private void CheckAdsResultFail(AdvertisementPostDTO adsDetail, UserBankAccount oldbankac, string failMsg)
        {
            setupFixture.CheckUserBankAccount(oldbankac, 0, 0, 0);
            setupFixture.CheckUserNotification(new int[] { adsDetail.UserId }, new string[] { failMsg },
                 AppSettings.AdsFailNotificationId, false, 1, 1);
            CheckAds(adsDetail, 0);

            adsDetail.CountryId = webRepo.GetCountryId(adsDetail.UserId);

            setupFixture.CheckCapitalTransactionLog(adsDetail.UserId,
                countryRepo.GetCountryCode(adsDetail.CountryId).CountryUserId,
                adsDetail.TotalCost,
                adsDetail.CalculatedTax,
                 AppSettings.AdsFundType, 0);
        }

        private void CheckAds(AdvertisementPostDTO adsDetial, int count)
        {
            string getadssql = string.Format("Select * from Advertisement Where UserId in  ({0})", adsDetial.UserId);
            List<Advertisement> ads = spContext.GetSqlData<Advertisement>(getadssql).ToList();

            Assert.AreEqual(ads.Count, count);
            Assert.AreEqual(ads.Count(f => f.AdsFrequencyTypeId == adsDetial.AdsFrequencyTypeId
                && f.EndDate == adsDetial.EndDate
                && f.StartDate == adsDetial.StartDate
                && f.Cost == adsDetial.TotalCost
                && f.PreviewMsg == adsDetial.PreviewMsg
                && f.Message == adsDetial.Message), count
                );
            if (count > 0)
            {

                foreach (var item in adsDetial.AdsTypeList)
                {
                    if (item == 1)
                    {
                        Assert.IsTrue(ads[0].AdsTypeEmail);
                    }
                    else if (item == 2)
                    {
                        Assert.IsTrue(ads[0].AdsTypeFeed);
                    }
                    else if (item == 3)
                    {
                        Assert.IsTrue(ads[0].AdsTypePartyMember);
                    }
                    else if (item == 4)
                    {
                        Assert.IsTrue(ads[0].AdsTypeCountryMember);
                    }
                }

                foreach (var item in adsDetial.Days)
                {
                    if (item == 0)
                    {
                        Assert.IsTrue(ads[0].DaysS);
                    }
                    else if (item == 1)
                    {
                        Assert.IsTrue(ads[0].DaysM);
                    }
                    else if (item == 2)
                    {
                        Assert.IsTrue(ads[0].DaysT);
                    }
                    else if (item == 3)
                    {
                        Assert.IsTrue(ads[0].DaysW);
                    }
                    else if (item == 4)
                    {
                        Assert.IsTrue(ads[0].DaysTh);
                    }
                    else if (item == 5)
                    {
                        Assert.IsTrue(ads[0].DaysF);
                    }
                    else if (item == 6)
                    {
                        Assert.IsTrue(ads[0].DaysSa);
                    }
                }
            }
        }
        #endregion Buy Ads
    }
}
