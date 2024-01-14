using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CountryCodeRepository : ICountryCodeRepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();

        public CountryCodeRepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public CountryCodeRepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }

        public string GetCountryCodes()
        {
            string CountryCodeData = cache.GetStringKey(AppSettings.RedisKeyCountryCodes);
            if (CountryCodeData == null)
            {
                CountryCodeData = JsonConvert.SerializeObject(spContext.GetSqlDataNoParms<CountryCode>(AppSettings.SPGetCountryCodeList));
                cache.SetStringKey(AppSettings.RedisKeyCountryCodes, CountryCodeData);
            }
            return (CountryCodeData);
        }
        public string GetCountryCodeJson(string countryId)
        {
            string countryName = cache.GetStringKey(AppSettings.RedisKeyCountryCodes + countryId);

            if (countryName == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", countryId);
                countryName = JsonConvert.SerializeObject(spContext.GetByPrimaryKey<CountryCode>(dictionary));
                cache.SetStringKey(AppSettings.RedisKeyCountryName + countryId, countryName);
            }
            return (countryName);
        }
        public CountryCode GetCountryCode(string countryId)
        {
            return JsonConvert.DeserializeObject<CountryCode>(
               GetCountryCodeJson(countryId));
        }
        public string GetCountryName(string countryId)
        {
            if (countryId == string.Empty)
            {
                return countryId;
            }
            string countryName = cache.GetHash(AppSettings.RedisHashCountryName, countryId);

            if (countryName == null)
            {
                CountryCode countrycode = JsonConvert.DeserializeObject<CountryCode>(
                GetCountryCodeJson(countryId));
                countryName = countrycode.Code;
                cache.SetHash(AppSettings.RedisHashCountryName, countryId, countrycode.Code);
            }
            return (countryName);
        }

        public int GetCountryPopulation(string countryCode)
        {
            double? countryPopulation = cache.GetSortedSetScore(AppSettings.RedisSortedSetCountryPopulation, countryCode.ToLower());
            if (countryPopulation == null)
            {
                PopulateCountryPopulation();

                countryPopulation = cache.GetSortedSetScore(AppSettings.RedisSortedSetCountryPopulation, countryCode.ToLower());

                if (countryPopulation == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(countryPopulation);
                }
            }
            else
            {
                return Convert.ToInt32(countryPopulation);
            }

        }

        public int GetCountryPopulationRank(string countryCode)
        {
            long? countryPopulationRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountryPopulation, countryCode.ToLower());
            if (countryPopulationRank == null)
            {
                PopulateCountryPopulation();

                countryPopulationRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountryPopulation, countryCode.ToLower());

                if (countryPopulationRank == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(countryPopulationRank);
                }
            }
            else
            {
                return Convert.ToInt32(countryPopulationRank);
            }
        }
        private void PopulateCountryPopulation()
        {
            IEnumerable<CountryPoulationDTO> countryPop = (spContext.GetSqlDataNoParms<CountryPoulationDTO>(AppSettings.SPGetCountryPopulationRank));

            cache.AddSoretedSets(AppSettings.RedisSortedSetCountryPopulation, countryPop.ToDictionary(x => x.CountryId.ToLower(), x => (Convert.ToDouble(x.TotalPopulation))));
            cache.ExpireKey(AppSettings.RedisSortedSetCountryPopulation, AppSettings.CountryProfileCacheLimit);

        }

        public string GetCountryRankingProfile(string countryId)
        {
            countryId = countryId.ToLower();
            string reidsKey = AppSettings.RedisHashCountryProfile + countryId;
            string countryprofileData = cache.GetHash(reidsKey, "ranking");
            if (countryprofileData == null)
            {
                CountryRankDTO[] rankDto = new CountryRankDTO[7];

                IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();
                IWeaponDetailsDTORepository weaponRepo = new WeaponDetailsDTORepository();
                IEducationDTORepository educationRepo = new EducationDTORepository();
                IJobDTORepository jobRepo = new JobDTORepository();
                ICountryBudgetDetailsDTORepository budgetRepo = new CountryBudgetDetailsDTORepository();

                Task<int> taskWealthRank =
                    Task<int>.Factory.StartNew(() => bankRepo.GetCountryCitizenWealthRank(countryId));
                Task<int> taskOffenseWeaponAssetRank =
                    Task<int>.Factory.StartNew(() => weaponRepo.GetCountryOffenseAssetRank(countryId));
                Task<int> taskDefenseWeaponAssetRank =
     Task<int>.Factory.StartNew(() => weaponRepo.GetCountryDefenseAssetRank(countryId));
                Task<int> taskSafestWeaponAssetRank =
          Task<int>.Factory.StartNew(() => budgetRepo.GetCountrySafestRank(countryId));

                Task<int> taskLiteracyRank =
              Task<int>.Factory.StartNew(() => educationRepo.GetCountryLiteracyRank(countryId));


                Task<int> taskPopulationRank =
                    Task<int>.Factory.StartNew(() => GetCountryPopulationRank(countryId));
                int avgSalaryRank = jobRepo.GetCountrySalaryRank(countryId);

                taskWealthRank.Wait();
                taskOffenseWeaponAssetRank.Wait();
                taskDefenseWeaponAssetRank.Wait();
                taskSafestWeaponAssetRank.Wait();
                taskLiteracyRank.Wait();
                taskPopulationRank.Wait();

                rankDto[0] = new CountryRankDTO();
                rankDto[1] = new CountryRankDTO();
                rankDto[2] = new CountryRankDTO();
                rankDto[3] = new CountryRankDTO();
                rankDto[4] = new CountryRankDTO();
                rankDto[5] = new CountryRankDTO();
                rankDto[6] = new CountryRankDTO();

                rankDto[0].Category = "Population";
                rankDto[0].Rank = taskPopulationRank.Result;
                rankDto[0].ImageFont = "fa fa-users";

                rankDto[1].Category = "Defense Asset";
                rankDto[1].Rank = taskDefenseWeaponAssetRank.Result;
                rankDto[1].ImageFont = "fa icon-war6";

                rankDto[2].Category = "Offense Asset";
                rankDto[2].Rank = taskOffenseWeaponAssetRank.Result;
                rankDto[2].ImageFont = "fa icon-airplane24";


                rankDto[3].Category = "Safest Nation";
                rankDto[3].Rank = taskSafestWeaponAssetRank.Result;
                rankDto[3].ImageFont = "fa fa-shield";

                rankDto[4].Category = "Average Salary";
                rankDto[4].Rank = avgSalaryRank;
                rankDto[4].ImageFont = "fa icon-online32";

                rankDto[5].Category = "Richest Citizens";
                rankDto[5].Rank = taskWealthRank.Result;
                rankDto[5].ImageFont = "fa icon-money28";


                rankDto[6].Category = "Literacy";
                rankDto[6].Rank = taskLiteracyRank.Result;
                rankDto[6].ImageFont = "fa icon-graduation22";



                countryprofileData = JsonConvert.SerializeObject(rankDto);

                cache.SetHash(reidsKey, "ranking", countryprofileData);
                cache.ExpireKey(reidsKey, AppSettings.CountryProfileCacheLimit);
            }

            return countryprofileData;
        }
        public string GetCountryProfileDTO(string countryId)
        {
            countryId = countryId.ToLower();
            string reidsKey = AppSettings.RedisHashCountryProfile + countryId;
            string countryprofileData = cache.GetHash(reidsKey, "profile");
            if (countryprofileData == null)
            {
                CountryProfileDTO profileDto = new CountryProfileDTO
                {
                    CountryId = countryId
                };
                ICountryBudgetDetailsDTORepository budgetRepo = new CountryBudgetDetailsDTORepository();
                Task<int> taskBudgetRank =
                    Task<int>.Factory.StartNew(() => budgetRepo.GetCountryBudgetRank(countryId));

                Task<int> taskPopulationRank =
                    Task<int>.Factory.StartNew(() => GetCountryPopulationRank(countryId));
                IWeaponDetailsDTORepository weaponRepo = new WeaponDetailsDTORepository();
                Task<int> taskWeaponAssetCount =
                    Task<int>.Factory.StartNew(() => weaponRepo.GetWeaponAssetCount(countryId));


                taskBudgetRank.Wait();
                profileDto.TotalBudget = budgetRepo.GetCountryBudgetValue(countryId);

                taskPopulationRank.Wait();
                taskWeaponAssetCount.Wait();

                profileDto.PopulationRank = taskPopulationRank.Result;
                profileDto.SecurityAsset = taskWeaponAssetCount.Result;
                profileDto.RichestCountryRank = taskBudgetRank.Result;


                countryprofileData = JsonConvert.SerializeObject(profileDto);

                cache.SetHash(reidsKey, "profile", countryprofileData);
                cache.ExpireKey(reidsKey, AppSettings.CountryProfileCacheLimit);
            }

            return countryprofileData;
        }
    }

}

