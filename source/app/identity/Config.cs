using System.Collections.Generic;
using identity.Models;
using IdentityServer4.Models;

namespace identity
{
    public static class Config
    {
        // declaration of possible scopes
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope
                {
                    Name = "customer.fullaccess",
                    Description = "some description over here"
                }
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "application_user",
                    AllowedGrantTypes = { "authorization_code" },
                    RequireClientSecret = false,
                    RedirectUris = { "urn:ietf:wg:oauth:2.0:oob" },
                    AllowedScopes = { "openid", "profile", "customer.fullaccess" },
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };

        public static IEnumerable<ApiResource> ApiResources { get; internal set; } = new List<ApiResource>
        {
            // what should be attached to cookie as claims
            new ApiResource
            {
                Name = "user",
                Scopes = { "customer.fullaccess" },
                UserClaims = { "role" }
            }
        };
        // what should be attached in scope
        public static IEnumerable<IdentityResource> IdentityResources { get; internal set; } = new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApplicationRole> Roles { get; internal set;} = new List<ApplicationRole>
        {
            new ApplicationRole { Name = "Admin" },
            new ApplicationRole { Name = "Moderator" },
            new ApplicationRole { Name = "Member" }
        };

        public static IEnumerable<ApplicationUser> Users { get; internal set;} = new List<ApplicationUser>
        {
            new ApplicationUser
            {
                Email = "admin@mock.com",
                UserName = "admin@mock.com",
            },
            new ApplicationUser
            {
                Email = "moderator@mock.com",
                UserName = "moderator@mock.com"
            }
        };
    }
}