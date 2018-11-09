using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCore.WebApi
{
    /// <summary>
    /// IdentityServerConfig
    /// </summary>
    public class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
                    {
                        new ApiResource("api1", "My Api")
                    };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
                        {
                            new Client()
                            {
                                ClientId = "client",
                                AllowedGrantTypes = GrantTypes.ClientCredentials,
                                ClientSecrets =
                                {
                                    new Secret("secret".Sha256())
                                },
                                AllowedScopes =
                                {
                                    "api1"
                                }
                            }
                        };
        }

    }
}
