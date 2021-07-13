using System.Collections.Generic;
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
                    Name = "catalog.fullaccess",
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
                    AllowedScopes = { "openid", "profile", "catalog.fullaccess" },
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };

        public static IEnumerable<ApiResource> ApiResources { get; internal set; } = new List<ApiResource>
        {
            // what should be attached to cookie as claims
            new ApiResource
            {
                Name = "user",
                Scopes = { "catalog.fullaccess" },
                UserClaims = { "role" }
            }
        };
        // what should be attached in scope
        public static IEnumerable<IdentityResource> IdentityResources { get; internal set; } = new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    }
}