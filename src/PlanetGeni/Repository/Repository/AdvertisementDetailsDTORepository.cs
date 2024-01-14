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
    public class AdvertisementDetailsDTORepository : IAdvertisementDetailsDTORepository
    {
        private StoredProcedure spContext = new StoredProcedure();
        private IRedisCacheProvider cache { get; set; }

        public AdvertisementDetailsDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public AdvertisementDetailsDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }

        public string GetAdsTypesJson()
        {
            string adscacheDTO = cache.GetStringKey(AppSettings.RedisKeyAdsDTO);
            if (adscacheDTO == null)
            {
                AdvertisementDTO adsDTO = new AdvertisementDTO();
                adsDTO.AdsTypeList =
                    spContext.GetSqlDataNoParms<AdsType>(AppSettings.SPGetAdsTypeList).ToArray();

                adsDTO.FrequencyAds =
                    spContext.GetSqlDataNoParms<AdsFrequencyType>
                    (AppSettings.SPGetAdsFrequencyTypeList).ToArray();

                adscacheDTO = JsonConvert.SerializeObject(adsDTO);
                cache.SetStringKey(AppSettings.RedisKeyAdsDTO, adscacheDTO);
            }
            return (adscacheDTO);
        }

        public void CalculateCost(ref AdvertisementPostDTO adsDetails)
        {
            decimal costTotal = 0;
            decimal taxTotal = 0;
            decimal total = 0;
            int adsLength = adsDetails.Message.Length;
            int totalDays = Convert.ToInt32((adsDetails.EndDate - adsDetails.StartDate).TotalDays) + 1;
            ICountryTaxDetailsDTORepository taxRepo = new CountryTaxDetailsDTORepository();
            AdvertisementDTO adsDTO = JsonConvert.DeserializeObject<AdvertisementDTO>(GetAdsTypesJson());

            foreach (var item in adsDetails.AdsTypeList)
            {
                AdsType adsType = adsDTO.AdsTypeList.First(f => f.AdsTypeId == item);
                costTotal += adsType.BaseCost;
                costTotal += adsDetails.ImageSize * adsType.PricePerImageByte;
                costTotal += adsLength * adsType.PricePerChar;
            }
            int frequencyTypeId = adsDetails.AdsFrequencyTypeId;
            AdsFrequencyType adsFrequency = adsDTO.FrequencyAds.First(f => f.AdsFrequencyTypeId == frequencyTypeId);
            int fqMultiple = adsFrequency.FrequencyMultiple;

            if (fqMultiple == 0)
            {
                fqMultiple = 1 + adsDetails.Days.Length;
            }
            if (totalDays > 5)
            {
                costTotal = costTotal * totalDays * totalDays * totalDays * fqMultiple;
            }
            else
            {
                costTotal = costTotal * totalDays * fqMultiple;
            }
            decimal taxRate = taxRepo.GetCountryTaxByCode(adsDetails.CountryId, AppSettings.TaxAdsCode);
            taxTotal = (taxRate / 100) * costTotal;
            total = costTotal + taxTotal;
            adsDetails.CalculatedTotalCost = total;
            adsDetails.CalculatedTax = taxTotal;


        }
        public bool SaveAds(AdvertisementPostDTO adsDetails)
        {

            bool result = false;
            try
            {
                adsDetails.AdvertisementId = Guid.NewGuid();
                if (PayAds(adsDetails))
                {
                    Advertisement adsPersist = new Advertisement();
                    foreach (var item in adsDetails.AdsTypeList)
                    {
                        if (item == 1)
                        {
                            adsPersist.AdsTypeEmail = true;
                        }
                        else if (item == 2)
                        {
                            adsPersist.AdsTypeFeed = true;
                        }
                        else if (item == 3)
                        {
                            adsPersist.AdsTypePartyMember = true;
                        }
                        else if (item == 4)
                        {
                            adsPersist.AdsTypeCountryMember = true;
                        }
                    }

                    foreach (var item in adsDetails.Days)
                    {
                        if (item == 0)
                        {
                            adsPersist.DaysS = true;
                        }
                        else if (item == 1)
                        {
                            adsPersist.DaysM = true;
                        }
                        else if (item == 2)
                        {
                            adsPersist.DaysT = true;
                        }
                        else if (item == 3)
                        {
                            adsPersist.DaysW = true;
                        }
                        else if (item == 4)
                        {
                            adsPersist.DaysTh = true;
                        }
                        else if (item == 5)
                        {
                            adsPersist.DaysF = true;
                        }
                        else if (item == 6)
                        {
                            adsPersist.DaysSa = true;
                        }
                    }


                    adsPersist.UserId = adsDetails.UserId;
                    adsPersist.Cost = adsDetails.CalculatedTotalCost;
                    adsPersist.AdTime = adsDetails.AdTime;
                    adsPersist.StartDate = adsDetails.StartDate;
                    adsPersist.EndDate = adsDetails.EndDate;
                    adsPersist.AdsFrequencyTypeId = adsDetails.AdsFrequencyTypeId;
                    adsPersist.Message = adsDetails.Message;
                    adsPersist.PreviewMsg = adsDetails.PreviewMsg;

                    adsPersist.AdvertisementId = adsDetails.AdvertisementId;
                    spContext.Add(adsPersist);
                    result = true;
                    return result;
                }
                else
                {
                    return result;

                }
            }
            catch (Exception ex)
            {
                result = false;
                ExceptionLogging.LogError(ex, "Error Saving Ads to repository");
                return result;

            }
        }

        private bool PayAds(AdvertisementPostDTO adsDetails)
        {
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                ICountryCodeRepository countryRepo = new CountryCodeRepository();



                dictionary.Add("parmUserId", adsDetails.UserId);
                dictionary.Add("parmCountryUserId", countryRepo.GetCountryCode(adsDetails.CountryId).CountryUserId);
                dictionary.Add("parmCountryId", adsDetails.CountryId);
                dictionary.Add("parmTaskId", adsDetails.AdvertisementId);
                dictionary.Add("parmFundType", AppSettings.AdsFundType);
                dictionary.Add("parmTaxCode", AppSettings.TaxAdsCode);
                dictionary.Add("parmAmount", adsDetails.CalculatedTotalCost - adsDetails.CalculatedTax);
                dictionary.Add("parmTaxAmount", adsDetails.CalculatedTax);

                int response = (int)spContext.GetSqlDataSignleValue
              (AppSettings.SPExecutePayNation, dictionary, "result");
                if (response != 1)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to PayAds");
                return false;
            }

        }
    }
}
