using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetApplication
    {
        public Guid ApplicationId { get; set; }
        [Required]
        [MaxLength(256)]
        public string ApplicationName { get; set; }
        [Required]
        [MaxLength(256)]
        public string LoweredApplicationName { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        public virtual ICollection<AspNetMembership> AspNetMemberships { get; set; }
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }

        public AspNetApplication()
        {
            this.AspNetUsers = new HashSet<AspNetUser>();
            this.AspNetMemberships = new HashSet<AspNetMembership>();
            this.AspNetRoles = new HashSet<AspNetRole>();
        }
    }
}
