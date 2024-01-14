using Common;
using DAO.Models;
using System;
namespace DTO.Custom
{
    public class SlotMachineThreeDTO
    {
        public SlotMachineThree[] SlotMachineList { get; set; }
        public decimal Match3Factor { get; set; }
        public decimal Match2Factor { get; set; }
        public decimal RouleteFactor { get; set; }
        public SlotMachineThreeDTO()
        {
            Match2Factor = RulesSettings.SlotMachine2MatchAwardFactor;
            Match3Factor = RulesSettings.SlotMachineAll3MatchAwardFactor;
            RouleteFactor = RulesSettings.RouleteMatchAwardFactor;
        }
    }
}
