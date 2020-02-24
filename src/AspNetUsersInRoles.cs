using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetUsersInRoles
    {
        public Guid UserID { get; set; }
        public Guid RoleID { get; set; }
        public virtual AspNetUser User { get; set; }
        public virtual AspNetRole Role { get; set; }
    }
}
