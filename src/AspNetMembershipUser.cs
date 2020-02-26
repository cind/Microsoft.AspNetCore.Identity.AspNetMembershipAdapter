using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetMembershipUser : IdentityUser<Guid>
    {
        public string PasswordSalt { get; set; }
        public int PasswordFormat { get; set; }
    }
}
