using Common;
using DAO;
using DAO.Models;
using DTO.Db;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Jobs
{
    public class LottoCheckManager
    {
        private PickFiveWinNumber pick5WinningNumber = new PickFiveWinNumber();
        private PickThreeWinNumber pick3WinningNumber = new PickThreeWinNumber();
        private List<NextLotteryDrawing> drawing = new List<NextLotteryDrawing>();
        private Random random = new Random();
        ILotteryDTORepository lotteryRepo = new LotteryDTORepository();
        ICountryTaxDetailsDTORepository countryTaxRepo = new CountryTaxDetailsDTORepository();
        IWebUserDTORepository webRepo = new WebUserDTORepository();
        private IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();
        private IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
        private IPostCommentDTORepository postRepo = new PostCommentDTORepository();

        public LottoCheckManager()
        {

        }
        public int CheckLottery(int runId)
        {
            drawing = JsonConvert.DeserializeObject<List<NextLotteryDrawing>>(
                lotteryRepo.GetNextLotteryDrawingDate());
            int winners = 0;
            GeneratePick3WinningNumber();
            GeneratePick5WinningNumber();
            if (drawing.Find(f => f.LotteryType == "T").NextDrawingDate <= DateTime.UtcNow)
            {
                winners += CheckPick3();

            }
            else
            {
                drawing.RemoveAll(f => f.LotteryType == "T");
            }
            if (drawing.Find(f => f.LotteryType == "F").NextDrawingDate <= DateTime.UtcNow)
            {
                winners += CheckPick5();
            }
            else
            {
                drawing.RemoveAll(f => f.LotteryType == "F");
            }
            AddWinningNumbers();
            SetupNextDrawing();
            Console.WriteLine("Total Number Of Winners {0}", winners);
            return winners;
        }
        private int CheckPick3()
        {
            IEnumerable<LotteryMatch> pick3Winners = lotteryRepo.GetPick3WinnersThisDrawing(pick3WinningNumber);
            decimal taxRate;
            decimal prize = 0;
            PickThreeWinner winner = new PickThreeWinner();
            winner.DrawingId = drawing.Find(f => f.LotteryType == "T").DrawingId;
            foreach (var item in pick3Winners)
            {
                prize = CalculatePrize(item.PickMatch, "T");
                if (prize == 0)
                {
                    continue;
                }
                string countryId = webRepo.GetCountryId(item.UserId);
                taxRate = countryTaxRepo.GetCountryTaxByCode(countryId, AppSettings.TaxLotteryCode);
                PayMeDTO payMe = new PayMeDTO
                {
                    ReciepentId = item.UserId,
                    SourceUserId = AppSettings.BankId,
                    CountryId = countryId,
                    TaskId = Guid.NewGuid(),
                    FundType = AppSettings.LotteryFundType,
                    TaxCode = (sbyte)AppSettings.TaxLotteryCode,
                    Amount = prize * (1 - taxRate / 100),
                    Tax = prize * taxRate / 100
                };
                bankRepo.PayMe(payMe);
                winner.UserId = item.UserId;
                winner.Amount = prize;
                lotteryRepo.SavePick3Winners(winner);
                AddNotificationandPost(item, "Three", prize);

            }

            Console.WriteLine("Total Number Of Pick 3 Winners {0}", pick3Winners.Count());
            return pick3Winners.Count();

        }

        private int CheckPick5()
        {
            IEnumerable<LotteryMatch> pick5Winners = lotteryRepo.GetPick5WinnersThisDrawing(pick5WinningNumber);
            decimal taxRate;
            decimal prize = 0;
            PickFiveWinner winner = new PickFiveWinner();
            winner.DrawingId = drawing.Find(f => f.LotteryType == "F").DrawingId;
            foreach (var item in pick5Winners)
            {
                prize = CalculatePrize(item.PickMatch, "F");
                if (prize == 0)
                {
                    continue;
                }
                string countryId = webRepo.GetCountryId(item.UserId);
                taxRate = countryTaxRepo.GetCountryTaxByCode(countryId, AppSettings.TaxLotteryCode);
                PayMeDTO payMe = new PayMeDTO
                {
                    ReciepentId = item.UserId,
                    SourceUserId = AppSettings.BankId,
                    CountryId = countryId,
                    TaskId = Guid.NewGuid(),
                    FundType = AppSettings.LotteryFundType,
                    TaxCode = (sbyte)AppSettings.TaxLotteryCode,
                    Amount = prize * (1 - taxRate / 100),
                    Tax = prize * taxRate / 100
                };
                bankRepo.PayMe(payMe);
                winner.UserId = item.UserId;
                winner.Amount = prize;
                lotteryRepo.SavePick5Winners(winner);
                AddNotificationandPost(item, "Five", prize);

            }

            Console.WriteLine("Total Number Of Pick 5 Winners {0}", pick5Winners.Count());
            return pick5Winners.Count();

        }

        private decimal CalculatePrize(Int64 pickMatch, string lotteryType)
        {
            if (lotteryType == "F")
            {
                switch (pickMatch)
                {
                    case 1:
                        return RulesSettings.LotteryPick5Match1;
                    case 2:
                        return RulesSettings.LotteryPick5Match2;
                    case 3:
                        return RulesSettings.LotteryPick5Match3;
                    case 4:
                        return RulesSettings.LotteryPick5Match4;
                    case 5:
                        return RulesSettings.LotteryPick5Match5;
                }
            }
            else if (lotteryType == "T")
            {
                switch (pickMatch)
                {
                    case 1:
                        return RulesSettings.LotteryPick3Match1;
                    case 2:
                        return RulesSettings.LotteryPick3Match2;
                    case 3:
                        return RulesSettings.LotteryPick3Match3;
                }
            }
            return 0;
        }
        private void SetupNextDrawing()
        {
            if (drawing.Count == 0)
            {
                return;
            }
            foreach (var item in drawing)
            {
                item.DrawingId++;
                if (item.LotteryType == "F")
                {
                    item.NextDrawingDate = item.NextDrawingDate.AddDays(RulesSettings.Pick5Frequency);
                    Console.WriteLine("Next Drawing date Pick Five {0}", item.NextDrawingDate);

                }
                else if (item.LotteryType == "T")
                {
                    item.NextDrawingDate = item.NextDrawingDate.AddDays(RulesSettings.Pick3Frequency);
                    Console.WriteLine("Next Drawing date Pick Three {0}", item.NextDrawingDate);

                }
            }
            lotteryRepo.SaveNextDrawing(drawing);
            Console.WriteLine("Next Drawing setup");
            lotteryRepo.ClearNextDrawingCache();
            Console.WriteLine("Cache Cleared");

        }
        private void AddWinningNumbers()
        {
            var lotteryThree = drawing.Find(f => f.LotteryType == "T");
            if (lotteryThree != null && lotteryThree.NextDrawingDate <= DateTime.UtcNow)
            {
                lotteryRepo.SavePick3WininingNumber(pick3WinningNumber);
                Console.WriteLine("Added Pick3 Winning Number {0} {1} {2} ",
                          pick3WinningNumber.Number1,
                          pick3WinningNumber.Number2,
                          pick3WinningNumber.Number3);
            }
            var lotteryFive = drawing.Find(f => f.LotteryType == "F");

            if (lotteryFive != null && lotteryFive.NextDrawingDate <= DateTime.UtcNow)
            {
                lotteryRepo.SavePick5WininingNumber(pick5WinningNumber);
                Console.WriteLine("Added Pick5 Winning Number {0} {1} {2} {3} {4} ",
                    pick5WinningNumber.Number1,
                    pick5WinningNumber.Number2,
                    pick5WinningNumber.Number3,
                    pick5WinningNumber.Number4,
                    pick5WinningNumber.Number5
                    );
            }
        }
        private void AddNotificationandPost(LotteryMatch item, string lotteryType, decimal prize)
        {
            WebUserDTO webUser = webRepo.GetUserPicFName(item.UserId);
            StringBuilder parmText = new StringBuilder();
            parmText.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
                item.UserId,
                webUser.Picture,
                webUser.FullName,
                lotteryType,
                prize,
                item.PickMatch
                );

            userNotif.AddNotification(false, string.Empty,
      AppSettings.LotteryWinNotificationId, parmText.ToString(), 1, item.UserId);

            if (prize > RulesSettings.LotteryCapForPost)
            {
                Post post = new Post
                {
                    Parms = parmText.ToString(),
                    PostContentTypeId = AppSettings.LotteryWinPostContentTypeId,
                    UserId = item.UserId,
                };
                postRepo.SavePost(post);
            }

        }
        private void GeneratePick3WinningNumber()
        {
            pick3WinningNumber.DrawingId = drawing.Find(f => f.LotteryType == "T").DrawingId;
            pick3WinningNumber.DrawingDate = drawing.Find(f => f.LotteryType == "T").NextDrawingDate;
            pick3WinningNumber.Number1 = (sbyte)random.Next(1, 11);
            pick3WinningNumber.Number2 = (sbyte)random.Next(1, 11);
            pick3WinningNumber.Number3 = (sbyte)random.Next(1, 11);

        }
        private void GeneratePick5WinningNumber()
        {
            pick5WinningNumber.DrawingId = drawing.Find(f => f.LotteryType == "F").DrawingId;
            pick5WinningNumber.DrawingDate = drawing.Find(f => f.LotteryType == "F").NextDrawingDate;
            pick5WinningNumber.Number1 = (sbyte)random.Next(1, 11);
            pick5WinningNumber.Number2 = (sbyte)random.Next(1, 11);
            pick5WinningNumber.Number3 = (sbyte)random.Next(1, 11);
            pick5WinningNumber.Number4 = (sbyte)random.Next(1, 11);
            pick5WinningNumber.Number5 = (sbyte)random.Next(1, 11);

        }
    }
}
