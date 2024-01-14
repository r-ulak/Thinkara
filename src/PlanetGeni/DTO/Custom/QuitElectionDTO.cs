using DAO.Models;
using System;
namespace DTO.Db
{
    public class QuitElectionDTO : WebUserDTO
    {
        public ElectionCandidate CandidateInfo{ get; set; }
    }


}
