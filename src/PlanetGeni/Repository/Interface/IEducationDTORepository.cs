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
    public interface IEducationDTORepository
    {

        void GiveEducationCreditForCountry();
        void PostNotifcation(DegreeCheckDTO degreeCheck);
        void PostGraduationConetent(DegreeCheckDTO item);

        IEnumerable<DegreeCheckDTO> DegreeCheck(int runId);

        string GetEducationProfile(int userId);
        string GetDegreeCodesJson();
        string GetMajorCodesJson();
        IEnumerable<EducationDTO> GetEducationByUserId(int userid);
        IEnumerable<EducationSummaryDTO> GetEducationSummary(int userid);
        string GetTopTenDegreeHolderJson();
        bool SaveEnrollDegree(EnrollDegreeDTO[] enrollDegreeList, int userid, decimal tax, string countryId);
        EducationBoostDTO ApplyNextBoost(EnrollDegreeDTO enrollDegree, int userid);
        int GetCountryLiteracyRank(string countryCode);
        void ClearCache();

    }
}
