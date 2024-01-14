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
    public class PartyRules : IRules
    {
        public StartPartyDTO NewPartyInfo;
        public int UserId;
        private PoliticalParty ClosingPoliticalParty;
        public PartyInviteDTO PartyInviteInfo;
        private ClosePartyDTO CloseParty;
        private DonatePartyDTO Donation;
        private QuitPartyDTO QuitParty;
        private PoliticalParty NominatingPoliticalParty;
        private PartyNominationDTO NominateParty;
        private EjectPartyDTO PartyEjection;
        public JoinRequestPartyDTO[] JoinParty;
        public PartyRules(QuitPartyDTO quitParty)
        {
            QuitParty = quitParty;
        }
        public PartyRules(DonatePartyDTO partydonation)
        {
            Donation = partydonation;
        }
        public PartyRules(PartyInviteDTO partyInviteDTO)
        {
            PartyInviteInfo = partyInviteDTO;
        }
        public PartyRules(EjectPartyDTO partyEjection)
        {
            PartyEjection = partyEjection;
        }
        public PartyRules(JoinRequestPartyDTO[] joinParty)
        {
            JoinParty = joinParty;
        }
        public PartyRules(PoliticalParty politicalParty, ClosePartyDTO closeParty)
        {
            ClosingPoliticalParty = politicalParty;
            CloseParty = closeParty;
        }
        public PartyRules(PartyNominationDTO nominateParty, PoliticalParty nominatingParty)
        {
            NominateParty = nominateParty;
            NominatingPoliticalParty = nominatingParty;
        }
        public PartyRules()
        {

        }
        public PartyRules(
            StartPartyDTO newPartyInfo)
        {
            NewPartyInfo = newPartyInfo;
        }

        public ValidationResult IsValid()
        {
            return ValidationResult.Success;
        }
        public ValidationResult IsValidPartyEjection()
        {
            if (PartyEjection.IsEjectorFounderorCoFounder == false)
            {
                return new ValidationResult((" ejector must be founder or cofounder."));
            }

            if ((PartyEjection.EjectorPartyId.ToString() != PartyEjection.PartyId))
            {
                return new ValidationResult(("ejector not being active member of current party"));
            }
            if ((PartyEjection.EjecteePartyId.ToString() != PartyEjection.PartyId))
            {
                return new ValidationResult(("ejectee not being active member of current party"));
            }
            if (PartyEjection.EjecteeMemberStatus != string.Empty)
            {
                return new ValidationResult(("member curretnly being nominated or ejected"));
            }
            if (PartyEjection.InitatorId == PartyEjection.EjecteeId)
            {
                return new ValidationResult(("you cannot eject yourself"));
            }
            return ValidationResult.Success;
        }
        public ValidationResult IsValidCloseParty()
        {
            if (ClosingPoliticalParty.Status == "C")
            {
                return new ValidationResult((" party is already closed."));
            }
            if (ClosingPoliticalParty.Status == "H")
            {
                return new ValidationResult((" party already has a requset for closure."));
            }

            if (ClosingPoliticalParty.PartyFounder != CloseParty.InitatorId)
            {
                return new ValidationResult((" only party founder can request a closure"));
            }
            return ValidationResult.Success;
        }
        public ValidationResult IsValidNominationParty()
        {
            if ((NominateParty.NomineeIdPartyId.ToString() != NominateParty.PartyId))
            {
                return new ValidationResult(("nominne already  being active member of diffrent party. Nominne needs to leave the other party membership for being eligible to be nominated"));
            }
            if ((NominateParty.InitatorPartyId.ToString() != NominateParty.PartyId))
            {
                return new ValidationResult(("you being active member of diffrent party. Need to leave the other party membership for being eligible to nominate"));
            }

            if (NominateParty.NomineeIdMemberType == NominateParty.NominatingMemberType)
            {
                return new ValidationResult(("member already in nominated postion"));
            }
            if (NominatingPoliticalParty.Status != "A")
            {
                return new ValidationResult(("party not being in active status"));

            }
            if (NominateParty.NomineeIdMemberStatus != string.Empty)
            {
                return new ValidationResult(("member curretnly being nominated or ejected"));
            }
            if (NominateParty.HasPendingNomination == true)
            {
                return new ValidationResult(("nominee currently having pending nomination for same party."));
            }
            double cofounderCap = AppSettings.PartyCofounderSizeMaxPercent * NominatingPoliticalParty.PartySize;
            if (Math.Max(cofounderCap, AppSettings.PartyCofounderSize) < NominatingPoliticalParty.CoFounderSize + 1)
            {
                return new ValidationResult((string.Format(" party already having maxed out its cofounder cap, party can only have maximum of {0} % of the total member or {1} whichever is greater", AppSettings.PartyCofounderSizeMaxPercent * 100, AppSettings.PartyCofounderSize)));

            }

            return ValidationResult.Success;
        }
        public ValidationResult IsValidCreateParty()
        {
            if (NewPartyInfo.IsActiveMemberOfDiffrentParty == true)
            {
                return new ValidationResult(("you being member of a diffrent party. You need to leave the current party membership before creating a new Party"));
            }

            if (NewPartyInfo.IsUniquePartyName == false)
            {
                return new ValidationResult(("Party Name was already taken"));
            }
            if ((NewPartyInfo.FriendInvitationList.Length + NewPartyInfo.ContactInvitationList.Length) < AppSettings.InitialPartySize)
            {
                return new ValidationResult(String.Format("you must have {0} or more CoFounders when starting a new Party", AppSettings.InitialPartySize));
            }

            return CheckStartPartyEidts();
        }
        public ValidationResult IsValidManageParty()
        {
            if (NewPartyInfo.IsUniquePartyName == false)
            {
                return new ValidationResult(("Party Name was already taken"));
            }
            return CheckStartPartyEidts();
        }

        private ValidationResult CheckStartPartyEidts()
        {
            if (NewPartyInfo.AgendaType == null)
            {
                return new ValidationResult(("Must select at least 1 agenda"));
            }
            if (NewPartyInfo.AgendaType.Length <= 0)
            {
                return new ValidationResult(("Must select at least 1 agenda"));
            }
            if (NewPartyInfo.PartyName.Length < 5 == true)
            {
                return new ValidationResult(("Party name not being at least 5 character"));
            }
            if (NewPartyInfo.Motto.Length < 5 == true)
            {
                return new ValidationResult(("Motto name not being at least 5 character"));
            }
            if (!(NewPartyInfo.MembershipFee < 1000000 && NewPartyInfo.MembershipFee > -1))
            {
                return new ValidationResult(("MembershipFee not being in the range of 1000000 and 0"));
            }
            return ValidationResult.Success;
        }
        public ValidationResult IsValidPartyDonation()
        {
            if (Donation.PartyStatus != "A")
            {
                return new ValidationResult(("party not being in active status"));

            }
            if (Donation.IsCurrentOrPastParty == false)
            {
                return new ValidationResult(("you not being current or past Party member"));
            }

            return ValidationResult.Success;
        }
        public void IsValidPartyInviteRequest()
        {
            foreach (InviteeDTO item in PartyInviteInfo.PartyInvites)
            {

                if (item.AlreadyCurrentPartyMember == true)
                {
                    item.IsValid =
                          new ValidationResult((" invitee being current party member"));
                }
                else if (item.HasPendingPartyInviteForCurrentParty == true)
                {
                    item.IsValid =
                        new ValidationResult((" invitee having a pending request to join the party"));
                }
                else
                {
                    item.IsValid = ValidationResult.Success;
                }
            }
        }
        public void IsValidJoinPartyRequest()
        {
            foreach (var item in JoinParty)
            {

                if (item.IsAlreadyCurrentPartyMember == true)
                {
                    item.IsValid =
                          new ValidationResult((" already a current party member."));
                }
                else if (item.HasPendingJoinRequest == true)
                {
                    item.IsValid =
                        new ValidationResult((" already has a pending request to join the party."));
                }
                else
                {
                    item.IsValid = ValidationResult.Success;
                }
            }

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
