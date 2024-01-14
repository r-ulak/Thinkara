using Common;
using DAO;
using DAO.Models;
using DTO.Custom;
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
    public class ElectionVotingManager
    {
        IJobDTORepository jobRepo = new JobDTORepository();
        private IElectionDTORepository eleRepo = new ElectionDTORepository();
        private ICountryCodeRepository countryRepo = new CountryCodeRepository();
        private ICountryLeaderRepository countryLeaderRepo = new CountryLeaderRepository();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();

        public ElectionVotingManager()
        {

        }
        public void StatVoteCount(int runId)
        {
            IEnumerable<Election> elections = eleRepo.GetLastNoVoteCountedElectionPeriod();
            ClearCache();
            foreach (var currentElection in elections)
            {
                if (currentElection.ElectionId == 0)
                {
                    continue;
                }
                int candidates = eleRepo.GetElectionCandiate(currentElection.CountryId, currentElection.ElectionId);
                if (candidates > 0)
                {
                    int leaderLimit = RulesSettings.SeneateSeatHardCap;
                    int totalPopulation = Convert.ToInt32(countryRepo.GetCountryPopulation(currentElection.CountryId));
                    if (totalPopulation * RulesSettings.SenetaorSeatCapPercent < RulesSettings.SeneateSeatHardCap)
                    {
                        leaderLimit = Convert.ToInt32(totalPopulation * RulesSettings.SenetaorSeatCapPercent);
                    }

                    VoteCountingDTO votignDTO
                        = new VoteCountingDTO
                        {
                            CountryCode = countryRepo.GetCountryCode(currentElection.CountryId),
                            ElectionId = currentElection.ElectionId,
                            Priority = 1,
                            LeaderLimit = leaderLimit,

                        };
                    if (eleRepo.ElectionVoteCounting(votignDTO))
                    {
                        AddNextEelction(currentElection);
                    }
                }
            }
            eleRepo.AddNextElectionTerm();
            eleRepo.ClearAllElectionTermCache();
            ClearCache();
            System.Threading.Thread.Sleep(5000);
            NotifyElection();
        }

        private void AddNextEelction(Election election)
        {
            election.ElectionId++;
            election.StartDate = DateTime.UtcNow;
            election.VotingStartDate = DateTime.UtcNow.AddDays(RulesSettings.NumberOfDaysToElection);

            election.EndDate = election.VotingStartDate.AddDays(RulesSettings.NumberOfDaysofElection);
            election.Fee = GetNextElectionFee(election);
            eleRepo.AddElection(election);
        }

        public decimal GetNextElectionFee(Election election)
        {
            return (RulesSettings.EelctionFee * (1 + (decimal)election.ElectionId / 100));
        }

        private void NotifyElection()
        {
            eleRepo.NotifyStartOfElectionPeroid();
            eleRepo.NotifyStartOfVotingPeroid();
            eleRepo.NotifyLastDayOfVotingPeroid();

        }

        private void ClearCache()
        {
            List<CountryCode> countries = JsonConvert.DeserializeObject<List<CountryCode>>(countryRepo.GetCountryCodes());
            foreach (var item in countries)
            {
                foreach (var leader in countryLeaderRepo.GetActiveSeneator(item.CountryId))
                {
                    webRepo.ClearCacheProfileUpdate(leader.UserId);
                }
                countryLeaderRepo.ClearCache(item.CountryId);
            }
        }
    }

}