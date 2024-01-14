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
    public interface ILotteryDTORepository
    {
        string GetNextLotteryDrawingDate();
        IEnumerable<Pick3WinDTO> GetMyThreePicks(int userId, int lastDrawingId);
        IEnumerable<Pick5WinDTO> GetMyFivePicks(int userId, int lastDrawingId);
        string GetPickThreeWinNumber(int lastDrawingId);
        int SavePick5(int userId, PickFive pickFive);
        int SavePick3(int userId, PickThree pickThree);
        string GetPickFiveWinNumber(int lastDrawingId);
        void SavePick3WininingNumber(PickThreeWinNumber pick3WinningNumber);
        void SavePick5WininingNumber(PickFiveWinNumber pick5WinningNumber);

        void SaveNextDrawing(List<NextLotteryDrawing> drawing);

        void ClearNextDrawingCache();
        IEnumerable<LotteryMatch> GetPick5WinnersThisDrawing(PickFiveWinNumber winnignNumbers);
        IEnumerable<LotteryMatch> GetPick3WinnersThisDrawing(PickThreeWinNumber winnignNumbers);
        void SavePick5Winners(PickFiveWinner pick5Winner);
        void SavePick3Winners(PickThreeWinner pick3Winner);
    }
}
