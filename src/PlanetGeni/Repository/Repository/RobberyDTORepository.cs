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
    public class RobberyDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();
        private IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();
        private IPostCommentDTORepository postRepo = new PostCommentDTORepository();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        private IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();

        public RobberyDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }
        public RobberyDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }
        public bool NotRobbedInLastNDayByUser(CrimeIncidentDTO incident)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmRobberyFrequencyonSamePersonCap", RulesSettings.RobberyFrequencyonSamePersonCap); dictionary.Add("parmUserId", incident.UserId);
            dictionary.Add("parmVictimId", incident.VictimId);


            int result = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPNotRobbedInLastNDayByUser, dictionary, "Result"));
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public int ReportSuspectToAuthority(CrimeIncidentDTO incident)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmWantedScoreFactor", RulesSettings.RobberySuspectReportWantedFactor);
            dictionary.Add("parmIncidentId", incident.IncidentId);
            dictionary.Add("parmSuspectId", incident.SuspectId);

            return spContext.ExecuteStoredProcedure(AppSettings.SPReportSuspectToAuthority, dictionary);
        }
        public bool ProcessRobberySuspect(CrimeIncidentDTO incident)
        {
            try
            {
                CrimeIncidentSuspect crimeIncidentSuspect = new CrimeIncidentSuspect
                {
                    IncidentId = incident.IncidentId,
                    SuspectId = incident.SuspectId,
                    UserId = incident.SuspectReportingUserId,
                };
                spContext.Add(crimeIncidentSuspect);
                if (ReportSuspectToAuthority(incident) > 0)
                {
                    cache.Invalidate(AppSettings.RedisKeyCrimeUserReport + incident.SuspectId.ToString());
                    NotifySuspectwantedLevel(incident);
                }
                if (incident.VictimId == incident.SuspectReportingUserId)
                {
                    AddIncidentPost(incident);
                }
                AddSuspectReportingNotification(incident);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ExecuteStealProperty");
                return false;

            }

        }
        private void NotifySuspectwantedLevel(CrimeIncidentDTO incident)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", incident.SuspectId);
            CrimeReport report =
            spContext.GetByPrimaryKey<CrimeReport>(dictionary);
            StringBuilder parmtext = new StringBuilder();
            parmtext.AppendFormat("{0}|{1}|{2}",
                report.WantedScore,
                RulesSettings.RobberyMaxWantedLevel,
                RulesSettings.RobberyAssetSeizePercent);
       
            userNotif.AddNotification(false, string.Empty,
AppSettings.JailTimeNotificationId, parmtext.ToString(), 9, incident.SuspectId);

        }

        public bool IncidentAlreadyReported(CrimeIncidentDTO incident)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmIncidentId", incident.IncidentId);
            dictionary.Add("parmUserId", incident.SuspectReportingUserId);
            dictionary.Add("parmSuspectId", incident.SuspectId);
            CrimeIncidentSuspect crimeIncidentSuspect = spContext.GetByPrimaryKey<CrimeIncidentSuspect>(dictionary);
            return crimeIncidentSuspect.UserId == incident.SuspectReportingUserId;
        }
        private void AddIncidentPost(CrimeIncidentDTO incident)
        {
            StringBuilder parm = new StringBuilder();
            WebUserDTO webUser = webRepo.GetUserPicFName(incident.VictimId);
            parm.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
                    incident.StolenAsset,
                    incident.VictimId,
                    webUser.Picture, webUser.FullName, incident.Amount.ToString("N"),
                    incident.IncidentId
                    );

            Post post = new Post
            {
                Parms = parm.ToString(),
                PostContentTypeId = AppSettings.FriendRobberyContentTypeId,
                UserId = incident.VictimId
            };
            postRepo.SavePost(post);
        }
        private void AddSuspectReportingNotification(CrimeIncidentDTO incident)
        {
            StringBuilder parm = new StringBuilder();
            WebUserDTO webUser = webRepo.GetUserPicFName(incident.VictimId);
            parm.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                    incident.StolenAsset,
                    incident.VictimId,
                    webUser.Picture, webUser.FullName, incident.Amount.ToString("N"),
                    RulesSettings.RobberyMaxWantedLevel,
                    RulesSettings.RobberyAssetSeizePercent
                    );
            userNotif.AddNotification(false, string.Empty,
AppSettings.SuspectReportingNotificationId, parm.ToString(), 9, incident.SuspectId);
        }

        public bool ExecutePickPocket(CrimeIncidentDTO incident)
        {
            incident.IncidentId = Guid.NewGuid();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", incident.UserId);
            dictionary.Add("parmVictimId", incident.VictimId);
            dictionary.Add("parmRobberId", AppSettings.OutLawId);
            dictionary.Add("parmFundType", AppSettings.RobberyFundType);
            dictionary.Add("parmTaskId", incident.IncidentId);
            dictionary.Add("parmAmount", incident.Amount);
            try
            {
                int response = (int)spContext.GetSqlDataSignleValue
(AppSettings.SPExecutePickPocket, dictionary, "result");

                if (response == 1)
                {
                    return true;
                }
                return false;


            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ExecutePickPocket");
                return false;
            }

        }


        public decimal GetMaxAllowedPickPocketing(int friendId)
        {
            return bankRepo.GetUserBankDetails(friendId).Cash * RulesSettings.MaxAllowedPickPocketPercent / 100;
        }

        public bool ExecuteStealProperty(CrimeIncidentDTO incident)
        {
            incident.IncidentId = Guid.NewGuid();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", incident.UserId);
            dictionary.Add("parmVictimId", incident.VictimId);
            dictionary.Add("parmRobberId", AppSettings.OutLawId);
            dictionary.Add("parmTaskId", incident.IncidentId);
            dictionary.Add("parmMerchandiseTypeId", incident.MerchandiseTypeId);
            dictionary.Add("parmQuantity", RulesSettings.RobberyMaxQuantity);
            try
            {
                int response = (int)spContext.GetSqlDataSignleValue
(AppSettings.SPExecuteStealProperty, dictionary, "result");
                if (response == 1)
                {
                    string reidsKey = AppSettings.RedisHashUserProfile + incident.VictimId.ToString();
                    cache.SetHash(reidsKey, "merchandise", null);
                    reidsKey = AppSettings.RedisHashUserProfile + incident.UserId.ToString();
                    cache.SetHash(reidsKey, "merchandise", null);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ExecuteStealProperty");
                return false;

            }
        }


        public void ExecuteCrimeWatchWantedJob()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmIncidentPerDayRate", RulesSettings.IncidentPerDayRate);
            dictionary.Add("parmDeductionPerDayRate", RulesSettings.DeductionPerDayRate);
            dictionary.Add("parmMaxWantedLevel", RulesSettings.RobberyMaxWantedLevel);

            spContext.ExecuteStoredProcedure(AppSettings.SPCrimeWatchWantedJob, dictionary);
        }

        public IEnumerable<InJailDTO> GetAllUserInJail()
        {

            return spContext.GetSqlDataNoParms<InJailDTO>(AppSettings.SPGetAllUserInJail);
        }
        public void ExecuteArrestUser(ArrestUserDTO arrestUser)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmNewNetWorth", arrestUser.ReducedNetWorth);
            dictionary.Add("parmUserId", arrestUser.UserId);

            spContext.ExecuteStoredProcedure(AppSettings.SPArrestUser, dictionary);
        }
        public string GetCrimeReportByIncident(Guid incident)
        {
            string crimeIncidentData = cache.GetStringKey(AppSettings.RedisKeyCrimeIncident + incident.ToString());
            if (crimeIncidentData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmIncidentId", incident);
                crimeIncidentData = JsonConvert.SerializeObject(
                     spContext.GetSqlDataSignleRow<ReportIncidentDTO>(
                AppSettings.SPGetCrimeReportByIncident,
                dictionary));
                cache.SetStringKey(AppSettings.RedisKeyCrimeIncident + incident.ToString(), crimeIncidentData,
                    AppSettings.CrimeReportDataCacheLimit);
            }
            return (crimeIncidentData);
        }
        public string GetCrimeReportByUser(int userId)
        {
            string crimeReportData = cache.GetStringKey(AppSettings.RedisKeyCrimeUserReport + userId.ToString());
            if (crimeReportData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userId);
                crimeReportData = JsonConvert.SerializeObject(
                     spContext.GetSqlDataSignleRow<CrimeReportDTO>(
                AppSettings.SPGetCrimeReportByUser,
                dictionary));
                cache.SetStringKey(AppSettings.RedisKeyCrimeUserReport + userId.ToString(), crimeReportData,
                    AppSettings.CrimeReportDataCacheLimit);
            }
            return (crimeReportData);
        }

    }
}
