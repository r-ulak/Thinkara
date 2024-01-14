using System;
namespace DTO.Custom
{
    public class UserEmailDTO : IEquatable<UserEmailDTO>
    {
        public int UserId { get; set; }
        public string EmailId { get; set; }
        public string NameFirst { get; set; }
        public bool Equals(UserEmailDTO other)
        {
            return UserId == other.UserId
                     && EmailId == other.EmailId
                     && NameFirst == other.NameFirst;
        }
        public override int GetHashCode()
        {
            return this.UserId.GetHashCode();
        }
    }
}
