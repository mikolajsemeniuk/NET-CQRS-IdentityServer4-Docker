using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace identity.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    // public class AppUser : IdentityUser<string>
    // {
    //     public ICollection<AppUserRole> UserRoles { get; set; }
    // }
}