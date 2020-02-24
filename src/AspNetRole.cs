using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetRole
    {
        public Guid ApplicationId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string LoweredRoleName { get; set; }
        public string Description { get; set; }
        public virtual AspNetApplication AspNetApplication { get; set; }
        public virtual ICollection<AspNetUsersInRoles> AspNetUsersInRoles { get; set; } = new HashSet<AspNetUsersInRoles>();

    }
}
