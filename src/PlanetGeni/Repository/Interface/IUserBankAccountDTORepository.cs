using Dao.Models;
using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserBankAccountDTORepository
    {
        int PayMe(PayMeDTO payMe);
        void ApplyCreditScore(decimal deltascore, int userId);
        int PayNation(PayNationDTO payNation);
        int GetCountryCitizenWealthRank(string countryCode);
        decimal GetNetWorth(int userId);
        string GetAllFundTypes();
        UserBankAccount GetUserBankDetails(int userId);
        bool UpdateBankAc(decimal delta, int userId);
        IEnumerable<BankViewDTO> GetBankAccountViewInfo(int userId);
        string GetTopTenRichestJson();
        IEnumerable<CapitalTransactionDTO> GetBankStatement
       (int userId, DateTime? parmlastDateTime = null);
        string GetMetalPrices();
        bool SaveBuySellMetalCart(BuySellMetalDTO metalCart);
    }
}
