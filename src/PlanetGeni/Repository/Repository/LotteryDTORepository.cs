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
    public class LotteryDTORepository : ILotteryDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();
        public LotteryDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }
        public LotteryDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }

        public string GetNextLotteryDrawingDate()
        {
            string nextDrawing = cache.GetStringKey(AppSettings.RedisKeyLotteryNextDrawing);
            if (nextDrawing == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                nextDrawing = JsonConvert.SerializeObject(
                     spContext.GetSqlData<NextLotteryDrawing>(
                AppSettings.SPGetNextLotteryDrawingDate,
                dictionary));
                cache.SetStringKey(AppSettings.RedisKeyLotteryNextDrawing, nextDrawing,
                    AppSettings.NextLotteryDrawCacheLimit);
            }
            return (nextDrawing);
        }

        public string GetPickFiveWinNumber(int lastDrawingId)
        {
            string reidsKey = AppSettings.RedisKeyPick5WinNumbers + lastDrawingId;
            string pickFiveWinNumber = cache.GetStringKey(reidsKey);
            if (pickFiveWinNumber == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmLimit", AppSettings.LotteryWinningNumberLimit);
                dictionary.Add("parmLastDrawingId", lastDrawingId);
                pickFiveWinNumber = JsonConvert.SerializeObject(
                     spContext.GetSqlData<PickFiveWinNumber>(
                AppSettings.SPGetPickFiveWinNumber,
                dictionary));
                cache.SetStringKey(reidsKey, pickFiveWinNumber,
                    AppSettings.NextLotteryDrawCacheLimit);
            }
            return (pickFiveWinNumber);
        }

        public string GetPickThreeWinNumber(int lastDrawingId)
        {
            string reidsKey = AppSettings.RedisKeyPick3WinNumbers + lastDrawingId;
            string pickThreeWinNumber = cache.GetStringKey(reidsKey);
            if (pickThreeWinNumber == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmLimit", AppSettings.LotteryWinningNumberLimit);
                dictionary.Add("parmLastDrawingId", lastDrawingId);
                pickThreeWinNumber = JsonConvert.SerializeObject(
                     spContext.GetSqlData<PickThreeWinNumber>(
                AppSettings.SPGetPickThreeWinNumber,
                dictionary));
                cache.SetStringKey(reidsKey, pickThreeWinNumber,
                    AppSettings.NextLotteryDrawCacheLimit);
            }
            return (pickThreeWinNumber);
        }


        public IEnumerable<Pick5WinDTO> GetMyFivePicks(int userId, int lastDrawingId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmLimit", AppSettings.LotteryWinningNumberLimit);
            dictionary.Add("parmLastDrawingId", lastDrawingId);
            IEnumerable<Pick5WinDTO> resultlist
                = spContext.GetSqlData<Pick5WinDTO>(AppSettings.SPGetMyFivePicks, dictionary);

            return resultlist;

        }


        public IEnumerable<Pick3WinDTO> GetMyThreePicks(int userId, int lastDrawingId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmLimit", AppSettings.LotteryWinningNumberLimit);
            dictionary.Add("parmLastDrawingId", lastDrawingId);
            IEnumerable<Pick3WinDTO> resultlist
                = spContext.GetSqlData<Pick3WinDTO>(AppSettings.SPGetMyThreePicks, dictionary);

            return resultlist;

        }

        public int SavePick3(int userId, PickThree pickThree)
        {
            try
            {
                List<NextLotteryDrawing> nextDrawing = JsonConvert.DeserializeObject<List<NextLotteryDrawing>>(
                    GetNextLotteryDrawingDate());
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmBuyerId", userId);
                dictionary.Add("parmBankId", AppSettings.BankId);
                dictionary.Add("parmDrawingId", nextDrawing.Find(z => z.LotteryType == "T").DrawingId);
                dictionary.Add("parmTotalValue", nextDrawing.Find(z => z.LotteryType == "T").LotteryPrice);
                dictionary.Add("parmFundType", AppSettings.LotteryFundType);
                dictionary.Add("parmNumber1", pickThree.Number1);
                dictionary.Add("parmNumber2", pickThree.Number2);
                dictionary.Add("parmNumber3", pickThree.Number3);

                int result = (int)spContext.GetSqlDataSignleValue
                    (AppSettings.SPExecutePick3LotteryOrder, dictionary, "result");
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SavePick3");
                return 2; //Error
            }
        }

        public int SavePick5(int userId, PickFive pickFive)
        {
            try
            {
                List<NextLotteryDrawing> nextDrawing = JsonConvert.
                    DeserializeObject<List<NextLotteryDrawing>>(
                    GetNextLotteryDrawingDate());
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmBuyerId", userId);
                dictionary.Add("parmBankId", AppSettings.BankId);
                dictionary.Add("parmDrawingId", nextDrawing.Find(z => z.LotteryType == "F").DrawingId);
                dictionary.Add("parmTotalValue", nextDrawing.Find(z => z.LotteryType == "F").LotteryPrice);
                dictionary.Add("parmFundType", AppSettings.LotteryFundType);
                dictionary.Add("parmNumber1", pickFive.Number1);
                dictionary.Add("parmNumber2", pickFive.Number2);
                dictionary.Add("parmNumber3", pickFive.Number3);
                dictionary.Add("parmNumber4", pickFive.Number4);
                dictionary.Add("parmNumber5", pickFive.Number5);

                int result = (int)spContext.GetSqlDataSignleValue
                    (AppSettings.SPExecutePick5LotteryOrder, dictionary, "result");
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SavePick3");
                return 2; //Error
            }
        }

        public void SavePick3WininingNumber(PickThreeWinNumber pick3WinningNumber)
        {
            spContext.Add(pick3WinningNumber);
        }
        public void SavePick5WininingNumber(PickFiveWinNumber pick5WinningNumber)
        {
            spContext.Add(pick5WinningNumber);
        }
        public void SavePick3Winners(PickThreeWinner pick3Winner)
        {
            spContext.Add(pick3Winner);
        }
        public void SavePick5Winners(PickFiveWinner pick5Winner)
        {
            spContext.Add(pick5Winner);
        }
        public void SaveNextDrawing(List<NextLotteryDrawing> drawing)
        {
            spContext.AddUpdateList(drawing);
        }
        public void ClearNextDrawingCache()
        {
            cache.Invalidate(AppSettings.RedisKeyLotteryNextDrawing);
        }

        public IEnumerable<LotteryMatch> GetPick5WinnersThisDrawing(PickFiveWinNumber winnignNumbers)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmDrawingId", winnignNumbers.DrawingId);
            dictionary.Add("parmNumber1", winnignNumbers.Number1);
            dictionary.Add("parmNumber2", winnignNumbers.Number2);
            dictionary.Add("parmNumber3", winnignNumbers.Number3);
            dictionary.Add("parmNumber4", winnignNumbers.Number4);
            dictionary.Add("parmNumber5", winnignNumbers.Number5);
                 
          return  spContext.GetSqlData<LotteryMatch>(
            AppSettings.SPGetMatchLottoPick5,
            dictionary);

        }
        public IEnumerable<LotteryMatch> GetPick3WinnersThisDrawing(PickThreeWinNumber winnignNumbers)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmDrawingId", winnignNumbers.DrawingId);
            dictionary.Add("parmNumber1", winnignNumbers.Number1);
            dictionary.Add("parmNumber2", winnignNumbers.Number2);
            dictionary.Add("parmNumber3", winnignNumbers.Number3);

            return spContext.GetSqlData<LotteryMatch>(
              AppSettings.SPGetMatchLottoPick3,
              dictionary);

        }
    }
}
