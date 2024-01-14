using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAdvertisementDetailsDTORepository
    {

        void CalculateCost(ref AdvertisementPostDTO adsDetails);
        string GetAdsTypesJson();
        bool SaveAds(AdvertisementPostDTO adsDetails);
    }
}
