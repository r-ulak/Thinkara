using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EducationDTORepository : IEducationDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();
        private IPostCommentDTORepository postRepo = new PostCommentDTORepository();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();

        public EducationDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public EducationDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }

        public string GetDegreeCodesJson()
        {
            string DegreeCodeData = cache.GetStringKey(AppSettings.RedisKeyDegreeCodes);
            if (DegreeCodeData == null)
            {
                DegreeCodeData = JsonConvert.SerializeObject(spContext.GetSqlDataNoParms<DegreeCode>(AppSettings.SPDegreeCodesList));
                cache.SetStringKey(AppSettings.RedisKeyDegreeCodes, DegreeCodeData);
            }
            return (DegreeCodeData);
        }

        public string GetMajorCodesJson()
        {
            string MajorCodeData = cache.GetStringKey(AppSettings.RedisKeyMajorCodes);
            if (MajorCodeData == null)
            {
                MajorCodeData = JsonConvert.SerializeObject(spContext.GetSqlDataNoParms<MajorCodeDTO>(AppSettings.SPMajorCodesList));
                cache.SetStringKey(AppSettings.RedisKeyMajorCodes, MajorCodeData);
            }
            return (MajorCodeData);
        }

        public IEnumerable<EducationDTO> GetEducationByUserId(int userid)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userid);
            IEnumerable<EducationDTO> userEducation =
         spContext.GetSqlData<EducationDTO>
         (AppSettings.SPGetEducationByUserId, dictionary);
            return userEducation;
        }
        public string GetEducationProfile(int userId)
        {
            string reidsKey = AppSettings.RedisHashUserProfile + userId;
            string profileEducationData = cache.GetHash(reidsKey, "education");
            if (profileEducationData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userId);
                profileEducationData = JsonConvert.SerializeObject(
                         spContext.GetSqlData<EducationProfileDTO>
                      (AppSettings.SPGetEducationProfile, dictionary));
                cache.SetHash(reidsKey, "education", profileEducationData);
                cache.ExpireKey(reidsKey, AppSettings.UserProfileCacheLimit);
            }
            return profileEducationData;

        }

        public string GetTopTenDegreeHolderJson()
        {
            string degreeTopNData = cache.GetStringKey(AppSettings.RedisKeyEducationSummaryTop10);
            if (degreeTopNData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmLimit", 10);
                degreeTopNData = JsonConvert.SerializeObject(
                     spContext.GetSqlData<TopTenDegreeHolderDTO>(
                AppSettings.SPGetTopNDegreeHolder,
                dictionary));
                cache.SetStringKey(AppSettings.RedisKeyEducationSummaryTop10, degreeTopNData,
                    AppSettings.EducationSummaryTop10CacheLimit);
            }
            return (degreeTopNData);
        }
        public IEnumerable<EducationSummaryDTO> GetEducationSummary(int userid)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userid);
            IEnumerable<EducationSummaryDTO> userEducationSummary =
         spContext.GetSqlData<EducationSummaryDTO>
         (AppSettings.SPGetEducationSummary, dictionary);
            return userEducationSummary;
        }

        public bool SaveEnrollDegree(EnrollDegreeDTO[] enrollDegreeList, int userid, decimal tax, string countryId)
        {
            bool result = false;
            try
            {
                decimal accountBalancedelta = 0;
                decimal totalTax = 0;
                List<Education> educationList = new List<Education>();
                foreach (EnrollDegreeDTO item in enrollDegreeList)
                {

                    Education userEducation = new Education
                    {
                        CompletionCost = item.Cost,
                        CreatedAt = item.CreatedAt,
                        DegreeId = (sbyte)item.DegreeId,
                        ExpectedCompletion = item.CreatedAt.AddHours(item.Duration * (1 + item.DegreeId)),
                        MajorId = (sbyte)item.MajorId,
                        NextBoostAt = item.CreatedAt.AddHours(AppSettings.NextBoostTime),
                        Status = item.Status,
                        UserId = userid
                    };
                    accountBalancedelta += item.Cost;
                    totalTax += item.Cost * tax / 100;
                    educationList.Add(userEducation);
                }

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmSourceId", userid);
                dictionary.Add("parmCountryId", countryId);
                dictionary.Add("parmTaskId", Guid.NewGuid());
                dictionary.Add("parmFundType", AppSettings.EducationFundType);
                dictionary.Add("parmTaxCode", AppSettings.TaxEducationCode);
                dictionary.Add("parmAmount", accountBalancedelta);
                dictionary.Add("parmTaxAmount", totalTax);

                int response = (int)spContext.GetSqlDataSignleValue
          (AppSettings.SPExecutePayWithTaxBank, dictionary, "result");

                if (response != 1)
                {
                    return false;
                }
                spContext.AddList(educationList);
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SaveEducationCart");
                return false;
            }
        }
        public EducationBoostDTO ApplyNextBoost(EnrollDegreeDTO enrollDegree, int userid)
        {
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userid);
                dictionary.Add("parmMajorId", enrollDegree.MajorId);
                dictionary.Add("parmDegreeId", enrollDegree.DegreeId);

                Education userEducation =
            spContext.GetByPrimaryKey<Education>(dictionary);

                if (userEducation.NextBoostAt <= DateTime.UtcNow)
                {
                    userEducation.NextBoostAt = DateTime.UtcNow.AddHours(AppSettings.NextBoostTime);
                    userEducation.ExpectedCompletion = userEducation.ExpectedCompletion.AddHours(AppSettings.BoostTime);
                    if (userEducation.ExpectedCompletion <= DateTime.UtcNow)
                    {
                        userEducation.Status = "C";
                        Task taskA = Task.Factory.StartNew(() => GraduationNotficationMsg
                            (userEducation.DegreeId, userEducation.MajorId, userid));

                    }
                }
                spContext.Update(userEducation);

                EducationBoostDTO educationBoost = new EducationBoostDTO
                {
                    NextBoostAt = userEducation.NextBoostAt,
                    ExpectedCompletion = userEducation.ExpectedCompletion,
                    Status = userEducation.Status
                };
                return educationBoost;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ApplyNextBoost");
            }
            return new EducationBoostDTO();
        }

        private void GraduationNotficationMsg(sbyte degreeId, short majorId, int userid)
        {
            try
            {


                List<MajorCodeDTO> majorCodes = JsonConvert.DeserializeObject<List<MajorCodeDTO>>(GetMajorCodesJson());
                List<DegreeCode> degreeCodes = JsonConvert.DeserializeObject<List<DegreeCode>>(GetDegreeCodesJson());
                DegreeCheckDTO degreeCheck = new DegreeCheckDTO();
                degreeCheck.MajorName = majorCodes.Find(x => x.MajorId == majorId).MajorName;
                degreeCheck.ImageFont = majorCodes.Find(x => x.MajorId == majorId).ImageFont;
                degreeCheck.DegreeName = degreeCodes.Find(x => x.DegreeId == degreeId).DegreeName;
                degreeCheck.DegreeImageFont = degreeCodes.Find(x => x.DegreeId == degreeId).DegreeImageFont;
                degreeCheck.UserId = userid;
                PostNotifcation(degreeCheck);
                PostGraduationConetent(degreeCheck);
            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to GraduationNotficationMsg");
            }

        }

        public void PostNotifcation(DegreeCheckDTO degreeCheck)
        {
            String parmText = "";
            parmText = string.Format("{0}|{1}", degreeCheck.DegreeName, degreeCheck.MajorName);
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            userNotif.AddNotification(false, string.Empty,
                                      AppSettings.EducationGraduationNotificationId,
                                      parmText, 2, degreeCheck.UserId);

        }
        public void PostGraduationConetent(DegreeCheckDTO item)
        {
            WebUserDTO userInfo = webRepo.GetUserPicFName(item.UserId);
            item.FullName = userInfo.FullName;
            item.Picture = userInfo.Picture;

            StringBuilder parm = new StringBuilder();
            parm.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                item.UserId,
                item.Picture,
                item.FullName,
                item.DegreeImageFont,
                item.DegreeName,
                item.ImageFont,
                item.MajorName
                );
            Post post = new Post
            {
                Parms = parm.ToString(),
                PostContentTypeId = AppSettings.GraduationPostContentTypeId,
                UserId=item.UserId
            };
            postRepo.SavePost(post);
        }
        public int GetCountryLiteracyRank(string countryCode)
        {
            long? countryLiteracyRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountryLiteracy, countryCode.ToLower());
            if (countryLiteracyRank == null)
            {
                PopulateCountryLiteracy();

                countryLiteracyRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountryLiteracy, countryCode.ToLower());

                if (countryLiteracyRank == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(countryLiteracyRank);
                }
            }
            else
            {
                return Convert.ToInt32(countryLiteracyRank);
            }
        }
        private void PopulateCountryLiteracy()
        {
            IEnumerable<CountryLiteracyDTO> countryPop = (spContext.GetSqlDataNoParms<CountryLiteracyDTO>(AppSettings.SPGetCountryLiteracyScore));

            cache.AddSoretedSets(AppSettings.RedisSortedSetCountryLiteracy, countryPop.ToDictionary(x => x.CountryId.ToLower(), x => (Convert.ToDouble(x.EducationScore))));
            cache.ExpireKey(AppSettings.RedisSortedSetCountryLiteracy, AppSettings.CountryProfileCacheLimit);

        }

        public IEnumerable<DegreeCheckDTO> DegreeCheck(int runId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmRunId", runId);
            return spContext.GetSqlData<DegreeCheckDTO>(AppSettings.SPDegreeCheck, dictionary);

        }

        public void GiveEducationCreditForCountry()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmEducationBudgetType", AppSettings.EducationBudgetType);
            dictionary.Add("parmNotificationTypeId", AppSettings.EducationCreditNotificationId);
            dictionary.Add("parmPostContentTypeId", AppSettings.EducationCreditContentTypeId);
            dictionary.Add("parmEducationFundType", AppSettings.EducationFundType);
             spContext.ExecuteStoredProcedure(AppSettings.SPGiveEducationCreditForCountry, dictionary);


        }
        public void ClearCache()
        {
            cache.Invalidate(AppSettings.RedisKeyDegreeCodes);
            cache.Invalidate(AppSettings.RedisKeyMajorCodes);
        }
    }
}
