using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace RulesEngine
{
    public class AdvertisementRules : IRules
    {
        public AdvertisementPostDTO AdsDetails { get; set; }
        public AdvertisementRules()
        {
        }
        public AdvertisementRules(AdvertisementPostDTO adsDetails)
        {
            AdsDetails = adsDetails;

        }
        public ValidationResult IsValid()
        {
            if (AdsDetails.Message.Length < 2 || AdsDetails.Message.Length > 1000)
                return new ValidationResult("Advertisement Message length must be between 5 and 1000");
            if ((AdsDetails.StartDate > AdsDetails.EndDate))
                return new ValidationResult("Advertisement StartDate cannot be greater than EndDate");
            if ((AdsDetails.StartDate < DateTime.UtcNow.Date))
                return new ValidationResult("Advertisement StartDate cannot be in past");
            if ((AdsDetails.AdsTypeList.Length == 0))
                return new ValidationResult("Ads Type is Required");
            if (!(AdsDetails.AdTime > 0 && AdsDetails.AdTime <= 23))
                return new ValidationResult("Ads Time is not in 24hr range");
            if (!(AdsDetails.AdsFrequencyTypeId >= 1 && AdsDetails.AdsFrequencyTypeId <= 5))
                return new ValidationResult("Ads Frequency was Invalid");
            if (AdsDetails.AdsFrequencyTypeId == 5)
            {
                if (AdsDetails.Days.Length == 0)
                    return new ValidationResult("Ads Days was must be selected");
            }

            return ValidationResult.Success;
        }

        public ValidationResult IsValidCost()
        {
            if ((AdsDetails.CalculatedTotalCost > AdsDetails.TotalCost))
                return new ValidationResult("cost calculated does not add up");

            return ValidationResult.Success;
        }




        public bool AllowUpdateInsert()
        {
            bool result = false;
            result = true;
            //TODO 
            //Check to see if they have access to Edit Ads then send 1 else 0.
            return result;
        }

    }
}
