using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetMembership
    {
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }


        [Required]
        [MaxLength(128)]
        public string Password { get; set; }
        public int PasswordFormat { get; set; }
        [Required]
        [MaxLength(128)]
        public string PasswordSalt { get; set; }
        [MaxLength(16)]
        public string MobilePIN { get; set; }
        [MaxLength(256)]
        public string Email { get; set; }
        [MaxLength(256)]
        public string LoweredEmail { get; set; }
        [MaxLength(256)]
        public string PasswordQuestion { get; set; }
        [MaxLength(128)]
        public string PasswordAnswer { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastLockoutDate { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public DateTime FailedPasswordAttemptWindowStart { get; set; }
        public int FailedPasswordAnswerAttemptCount { get; set; }
        public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
        public string Comment { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetApplication AspNetApplication { get; set; }
    }
}
