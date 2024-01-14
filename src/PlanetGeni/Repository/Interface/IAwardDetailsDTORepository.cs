using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAwardDetailsDTORepository
    {

        IQueryable<AchievementDTO> GetAchievementDTO(int userid, string achievementType);
        AwardSummaryDTO GetAwardSummaryDTO(int userid);

    }
}
