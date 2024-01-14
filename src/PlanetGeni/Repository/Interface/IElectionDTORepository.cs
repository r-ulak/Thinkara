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
    public interface IElectionDTORepository
    {
        void NotifyLastDayOfVotingPeroid();
        void NotifyStartOfVotingPeroid();
        void NotifyStartOfElectionPeroid();
        void ClearAllElectionTermCache();
        void AddNextElectionTerm();
        void ClearElectionTermCache(string countyrId);
        void UpdateElection(Election election);
        void AddElection(Election election);
        IEnumerable<Election> GetLastNoVoteCountedElectionPeriod();
        bool ElectionVoteCounting(VoteCountingDTO votignDTO);
        int GetElectionCandiate(string countryId, int electionId);
        bool HasVotedThisElection(CandidateVotingDTO voting);
        bool SaveVoting(CandidateVotingDTO votes);
        string GetElectionCandidateCountry(CandidateVotingDTO candidates);
        string GetCurrentVotingInfo(CandidateVotingDTO candidateVoting);
        bool QuitElection(QuitElectionDTO quit);
        ElectionCandidate GetElectionCandidate(int userId, int electionId, string countryId);
        int[] GetCandidateAgenda(int userId, int electionId);
        IEnumerable<ElectionVotingDTO> GetElectionResult(int userId);
        bool ExecuteDonateElection(PayWithTaxDTO donation);
        string GetCandidateByElection(CandidateSearchDTO candidateSearch);
        string GetElectionLast12Json(string countyrId);
        string GetPoliticalPostionsJson();
        Election GetCurrentElectionTerm(string countyrId);
        string GetCurrentElectionTermJson(string countyrId);
        bool HasPendingOrApprovedApplication(int userId, int electionId);
        int NumberofApprovedPartyMembers(int electionId, string partyId);
        int NumberOfApprovedCandidate(int electionId, string countryId);
        int GetConsecutiveTerm(int userId, int electionId);
        ElectionCandidate GetElectionCandidate(Guid taskId);
        bool ApplyForElection(RunForOfficeDTO runforOffice);
        void ProcessApprovedRunforOffice(ElectionCandidate candidate, string parmtext);
        void ProcessDeniedRunforOffice(ElectionCandidate candidate, string parmtext);
        RunForOfficeTicketDTO GetRunForOfficeTicket(string taskId, int userId);
    }
}
