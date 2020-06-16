// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityService.API
{
    public static class Config
    {
        // Clients and Resources can be loaded from data store. It's exactly implementation that we will use.
        // But at this point, we use in-memory version.
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("HowllowApi", "Howllow Service APIs")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {                
                new Client
                {
                    ClientId = "AutoproffFrontend",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("AutoproffFrontend".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "HowllowApi" }
                }
            };
    }
}