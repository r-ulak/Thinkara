using DAO;
using DAO.Models;
using DTO.Db;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Jobs
{
    public class DegreeCheckManager
    {
        public DegreeCheckManager()
        {
        
        }
        public  int StartDegreeCheck(int runId)
        {
            IEducationDTORepository educationRepo = new EducationDTORepository();
            IEnumerable<DegreeCheckDTO> degreeChecks =
            educationRepo.DegreeCheck(runId);
            foreach (var item in degreeChecks)
            {
                educationRepo.
                 PostNotifcation(item);
                educationRepo.
               PostGraduationConetent(item);
            }
            return degreeChecks.Count();
        }
    }
}
