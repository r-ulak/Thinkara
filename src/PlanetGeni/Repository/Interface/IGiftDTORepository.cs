using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IGiftDTORepository
    {
        bool SaveSendGift(GiftDTO gifts,
              UserBankAccount bankAc,
              int userid, GiftRateDTO giftRate, string fullName);
        GiftRateDTO GetGiftRate(string countryId);
        bool SaveSendPropertyGift(GiftDTO gifts,
    UserBankAccount bankAc,
    int userid, GiftRateDTO giftRate, string fullName);
    }
}
