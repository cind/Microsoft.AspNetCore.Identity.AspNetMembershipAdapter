using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetUser
    {
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(256)]
        public string LoweredUserName { get; set; }

        [MaxLength(16)]
        public string MobileAlias { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime LastActivityDate { get; set; }
        public virtual AspNetMembership AspNetMembership { get; set; }
        public virtual AspNetApplication AspNetApplication { get; set; }
        public virtual ICollection<AspNetUsersInRoles> AspNetUsersInRoles { get; set; } = new HashSet<AspNetUsersInRoles>();
    }
}
