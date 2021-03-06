using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace identity.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}