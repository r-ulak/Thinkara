using Common;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    public class ElectionRules : IRules
    {

        public ElectionRules()
        {

        }

        public ValidationResult IsValid()
        {
            return ValidationResult.Success;
        }

        public ValidationResult IsValidElectionApplication(RunForOfficeDTO runforOffice)
        {
            if (runforOffice.HasPendingApplication)
            {
                return new ValidationResult(("you currently have a pending or Approved application to run for this election term"));
            }
            if (runforOffice.TotalCash < runforOffice.CurrentTerm.Fee)
            {
                return new ValidationResult(("you do not have enough Cash to pay for elction fee."));
            }

            if ((runforOffice.FriendSelected.Length < RulesSettings.MinmumFriendsEndorsement) && runforOffice.CandidateTypeId == "I")
            {
                return new ValidationResult(
                               string.Format("number of Friends endorsement must be at least {0}", RulesSettings.MinmumFriendsEndorsement));
            }

            if (runforOffice.ConsecutiveTerm >= RulesSettings.MaxConsecutiveTerm)
            {
                return new ValidationResult(
                string.Format("you cannot run for election after being elected in last {0} consecutive terms", RulesSettings.MaxConsecutiveTerm));
            }

            if (runforOffice.NumberOfApprovedCandidate > RulesSettings.ElectionCandidateNumberHardCap)
            {
                return new ValidationResult(
                    string.Format("{0} members have already been approved to run in this election term, which is the curret elction Cap", RulesSettings.ElectionCandidateNumberHardCap));

            }
            if (runforOffice.NumberOfApprovedCandidate > RulesSettings.ElectionCapPercent * runforOffice.TotalPopulation)
            {
                return new ValidationResult(
                    string.Format("{0}% of total population has already been approved to run in this election term", RulesSettings.ElectionPartyCapPercent * 100));

            }

            if (!(runforOffice.Agendas.Length >= RulesSettings.MinmumAgenda && runforOffice.Agendas.Length <= RulesSettings.MaximumAgenda))
            {
                return new ValidationResult(
                               string.Format("number of Election Agenda must be between {0} and {1}", RulesSettings.MinmumAgenda, RulesSettings.MaximumAgenda));
            }

            if (runforOffice.CandidateTypeId == "P")
            {
                if (new Guid(runforOffice.PartyId) == Guid.Empty)
                {
                    return new ValidationResult(("you are not current member of the selcted party"));
                }
                if (runforOffice.NumberofApprovedPartyMembers > RulesSettings.ElectionPartyCapPercent * runforOffice.PartySize)
                {
                    return new ValidationResult(
                        string.Format("you already have {0}% of your Party Members approved to run in this election term ", RulesSettings.ElectionPartyCapPercent * 100));
                }
                if (runforOffice.PartyStatus != "A")
                {
                    return new ValidationResult(("your Party is not in approved status"));
                }


            }
            return ValidationResult.Success;
        }
        public ValidationResult IsValidElectionDonation(ElectionCandidate candidate)
        {
            if (candidate.Status != "A")
            {
                return new ValidationResult(("member is not currently Approved"));

            }

            return ValidationResult.Success;
        }

        public ValidationResult IsValidElectionVoting(CandidateVotingDTO voting)
        {
            if (voting.Candidates.Length > RulesSettings.MaximumElectionVotingCandidate)
            {
                return new ValidationResult(string.Format("you cannot vote for more than {0} candidates", RulesSettings.MaximumElectionVotingCandidate));
            }
            if (voting.Candidates.Length == 0)
            {
                return new ValidationResult("you need to vote for atleas one election candidate");
            }

            if (voting.UserCountryId == string.Empty)
            {
                return new ValidationResult("candidate was not found");
            }
            if (voting.CountryId.ToLower() != voting.UserCountryId.ToLower())
            {
                return new ValidationResult("you can only vote for candidates on your own country");
            }
            if (voting.HasVotedThisElection)
            {
                return new ValidationResult("you can only vote once every election sesion");
            }
            if (!(voting.ElectionInfo.VotingStartDate <= DateTime.UtcNow && DateTime.UtcNow <= voting.ElectionInfo.EndDate))
            {
                return new ValidationResult("voting has not begun");
            }
            return ValidationResult.Success;
        }
        public ValidationResult IsValidElectionQuit(ElectionCandidate candidate)
        {
            if (candidate.Status != "A")
            {
                return new ValidationResult(("member is not currently Approved"));

            }

            return ValidationResult.Success;
        }

        public bool AllowUpdateInsert()
        {
            bool result = false;
            result = true;
            //TODO 
            //Check to see if they have access to Edit PostComment then send 1 else 0.
            return result;
        }

    }
}
