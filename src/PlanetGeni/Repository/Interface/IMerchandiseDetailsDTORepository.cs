using DAO.Models;
using DTO.Custom;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IMerchandiseDetailsDTORepository
    {
        MerchandiseType GetMerchandiseType(short merchandiseType);
        int UpdateMerchandiseCondition();
        int ProcessUserWithRentalProperty(string countryId, decimal taxRate);
        int ProcessUserWithoutHouse(string countryId, int countryUserId);
        List<MerchandiseCache> GetMerchandiseCodesById(short[] typeIds);
        string GetMerchandiseCodesJson(MerchandiseCodeSearchDTO merchandiseCode);
        string GetMerchandiseProfile(int userId);
        void ClearCache();
        bool SaveMerchandiseCart(BuySellMerchandiseDTO[]
            merchandiseCartList, int userId, string countryId);
        IEnumerable<MerchandiseInventoryDTO> GetMerchandiseInventory(int userId, int lastMerchandiseTypeId);
        IEnumerable<MerchandiseSummaryDTO> GetMerchandiseSummaryJson(int userId);
        string GetTopTenPropertyOwnerJson();
        bool HasThisMerchandise(int userId, short[] items);
        bool SaveSellMerchandiseCart(BuySellMerchandiseDTO[]
  sellingItems, int userId, string countryId);
        UserMerchandise GetUserMerchandises(BuySellMerchandiseDTO
          merchandiseCart, int userId);
        decimal GetMerchandiseTotal(int userId, short[] items);
        int GetMerchandiseByQty(int userId, short[] items, int qty);

    }
}
