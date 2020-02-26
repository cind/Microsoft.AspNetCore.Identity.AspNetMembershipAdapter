using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetRole
    {
        public Guid ApplicationId { get; set; }
        public Guid RoleId { get; set; }
        
        [Required]
        [MaxLength(256)]
        public string RoleName { get; set; }

        [Required]
        [MaxLength(256)]
        public string LoweredRoleName { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }
        public virtual AspNetApplication AspNetApplication { get; set; }
        public virtual ICollection<AspNetUsersInRoles> AspNetUsersInRoles { get; set; } = new HashSet<AspNetUsersInRoles>();

    }
}
