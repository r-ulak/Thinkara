using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IPartyDTORepository
    {

        string GetAllUserParty(int userId);
        string GetlogoPictureId(string partyId);
        int GetpartySize(string partyId);
        string GetMyParties(int userId);
        string GetpartyStatus(string partyId);
        bool ManagePartyUploadLogo(StartPartyDTO startParty);
        void ExpireCachePoliticalParty(string partyId, int userId);
        void UpdatePartyStatus(string partyId, string status);
        string[] GetAllPartyMember(string partyId);
        bool CloseParty(PartyCloseRequest partyClose);
        PartyCloseRequest GetPartyCloseRequest(string taskId);
        IEnumerable<MyPoliticalPartyDTO> GetUserParties(int userId);
        IEnumerable<MyPoliticalPartyDTO> GetPastUserParties(int userId);
        PartyEjection GetPartyEjection(string taskId);
        PartyNomination GetPartyNomination(string taskId);
        string GetAllPoliticalAgendaJson();
        string GetPartyAgendasJson(string partyId);
        string GetPartyCoFounders(string partyId);
        string GetpartyName(string partyId);
        PartyMember GetActiveUserParty(int userId);
        string GetPartyMemberType(int userId);
        bool HasPendingNomination(string partyId, int userId);
        bool HasPendingPartyInivite(string partyId, int userId);
        bool HasPendingPartyInivite(string partyId, string emailId);
        string GetPartyMembers(GetPartyMemberTypeDTO partyMemberType);
        bool RequestEjectPartyMember(EjectPartyDTO ejectionDto);
        bool ExecuteDonateParty(DonatePartyDTO donation);
        PartySummaryDTO GetUserPartySummary(int userid);
        bool LeaveParty(QuitPartyDTO quit, PoliticalParty partyInfo);
        IEnumerable<WebUserContactDTO> GetEmailInvitationList(int userid, string lastemailId);
        string GetTopTenPartyByMember();
        bool IsActiveMemberOfDiffrentParty(int userId);
        bool IsActiveMemberOfParty(string partyId, int userId);
        bool IsCurrentOrPastParty(int userId, string partyId);
        bool SendPartyInvite(InviteeDTO partyInviteInfo, string partyId, int InitatorId, string fullName);
        string SearchParty(PartySearchDTO searchCriteria);
        bool IsUniquePartyName(string partyName, string countryId);
        bool StartParty(StartPartyDTO startParty);
        bool IsPartyFounder(int userId);
        bool IsPartyCoFounder(int userId);
        PoliticalParty GetPartyById(string partyId);
        string GetPartyByIdJson(string partyId);

        bool RequestCloseParty(ClosePartyDTO closeParty);
        bool RequestNominationPartyMember(PartyNominationDTO nominationParty, Guid taskId);
        bool NotifyNominationPartyMember(PartyNominationDTO nominationParty);

        string GetAllPartyMemberJson(string partyId);
        bool HasPendingJoinRequest(string partyId, int userId);
        bool RequestJoinParty(JoinRequestPartyDTO joinParty);
        PartyJoinRequest GetPartyJoinRequest(string taskId);
        int GetPartyMemberTotal(string partyId);
        bool SendApprovedMemeberPartyInvite(PartyJoinRequest joinRequest);
        PartyInvite GetPartyInviteById(string taskId);
        bool AddPartyMemberOnJoinRequest(PartyInvite invite, Guid taskId, bool hasJoinRequest = true);
        bool AddPartyFund(decimal amount, Guid partyId, int userId);
        void AddWelcomePost(PartyInvite invite, PoliticalParty partyInfo);
        string GetActivePartyId(int userId);
        void SetActivePartyId(string partyId, int userId);
        bool ProcessNominationApproval(PartyNomination nomination);
        void UpdateMemberStatus(int userId, string partyId, string status);
        string GetMemberStatus(int userId);
        bool IsPartyFounderOrCoFounder(int userId);
        void ProcessEjectionApproval(PartyEjection partyEjection);
        void ProcessEjectionDenial(PartyEjection partyEjection);
        void UpdateNomination(PartyNomination nomination, string status);
        void DecliedPartyMemberOnJoinRequest(PartyInvite invite, Guid taskId, bool hasJoinRequest = true);
        bool ManageParty(StartPartyDTO startParty);

    }
}
