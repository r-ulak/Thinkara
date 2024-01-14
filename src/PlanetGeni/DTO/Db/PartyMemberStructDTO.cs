using System;
using DAO.Models;
using System.Collections.Generic;
namespace DTO.Db
{
    public class PartyMemberStructDTO 
    {
        public PartyMemberDTO Founder { get; set; }
        public List<PartyMemberDTO> CoFounder { get; set; }
        public List<PartyMemberDTO> Members { get; set; }
        
    }
}
