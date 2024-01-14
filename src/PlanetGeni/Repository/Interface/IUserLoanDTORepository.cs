using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserLoanDTORepository
    {
        IQueryable<UserLoanDTO> GetLoanList
            (int userId, DateTime? lastLendorUpdatedAt = null,
            DateTime? lastBorrowerUpdatedAt = null);
        bool MakePayment(UserLoanDTO loanPayment, int userId);

        bool SaveLoanRequest(RequestLoanDTO requestloan, Guid
            taskId, int requestoruserId, string fullName);
        RequestLoanDTO GetQuailfiedIntrestedRate(int userId);
        UserLoan GetLoanById(Guid taskId);
        int UpdateLoanVoteResponse(VoteResponseDTO voteResponse);


    }
}
