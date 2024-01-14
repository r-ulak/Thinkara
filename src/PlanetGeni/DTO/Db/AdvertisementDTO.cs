
using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DTO.Db
{
    public  class AdvertisementDTO
    {
        public AdvertisementDTO()
        {

            TimeCodes = Enumerable.Range(0, 24).ToArray();
            DaysInWeek = new String[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        }
        public int[] TimeCodes { get; set; }
        public AdsType[] AdsTypeList { get; set; }
        public String[] DaysInWeek { get; set; }
        public AdsFrequencyType[] FrequencyAds { get; set; }
    }

}
