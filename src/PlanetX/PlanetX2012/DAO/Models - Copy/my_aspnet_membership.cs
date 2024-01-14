using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class my_aspnet_membership
    {
        public int userId { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string Password { get; set; }
        public string PasswordKey { get; set; }
        public Nullable<sbyte> PasswordFormat { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<System.DateTime> LastActivityDate { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<System.DateTime> LastPasswordChangedDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<bool> IsLockedOut { get; set; }
        public Nullable<System.DateTime> LastLockedOutDate { get; set; }
        public Nullable<long> FailedPasswordAttemptCount { get; set; }
        public Nullable<System.DateTime> FailedPasswordAttemptWindowStart { get; set; }
        public Nullable<long> FailedPasswordAnswerAttemptCount { get; set; }
        public Nullable<System.DateTime> FailedPasswordAnswerAttemptWindowStart { get; set; }
    }
}
