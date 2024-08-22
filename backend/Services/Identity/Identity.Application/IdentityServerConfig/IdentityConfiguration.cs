using IdentityModel;
using IdentityServer4.Models;
using static IdentityModel.OidcConstants;
using GrantTypes = IdentityServer4.Models.GrantTypes;

namespace Identity.Application.IdentityServerConfig
{
    public static class IdentityConfiguration
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new()
                {
                    Name = "roles",
                    DisplayName = "Roles",
                    UserClaims =
                    {
                        JwtClaimTypes.Role
                    }
                },
                new()
                {
                    Name = "id",
                    DisplayName = "Id",
                    UserClaims =
                    {
                        JwtClaimTypes.Id
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new("SwaggerScope"),
                new("MessageScope"),
                new("ListenerScope")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new("SwaggerAPI"),
                new("MessageAPI", new[]
                {
                    JwtClaimTypes.Id,
                    JwtClaimTypes.Role
                })
                {
                    Scopes = new List<string>
                    {
                        "MessageScope"
                    },
                    ApiSecrets = new List<Secret>
                    {
                        new("messagesecret".Sha256())
                    }
                },
                new("ListenerAPI", new[]
                {
                    JwtClaimTypes.Id,
                    JwtClaimTypes.Role
                })
                {
                    Scopes = new List<string>
                    {
                        "ListenerScope"
                    },
                    ApiSecrets = new List<Secret>
                    {
                        new("listenersecret".Sha256())
                    }
                }
            };

        public static IEnumerable<Client> Clients =>
    new List<Client>
    {
        new Client
        {
            ClientId = "client_id_swagger",
            ClientSecrets = { new Secret("client_secret_swagger".Sha512()) },
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            AllowedScopes = { "SwaggerAPI", StandardScopes.OpenId, StandardScopes.Profile }
        },
        new Client
        {
            ClientId = "api",
            ClientName = "ClientApi",
            AllowAccessTokensViaBrowser = true,
            ClientSecrets = new[]
            {
                new Secret("clientsecret".Sha512())
            },
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            AllowedScopes =
            {
                StandardScopes.OpenId,
                "MessageScope",
                "ListenerScope"
            }
        }
    };
    }
}
