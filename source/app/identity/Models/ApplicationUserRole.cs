using System;
using Microsoft.AspNetCore.Identity;

namespace identity.Models
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }
    }
}