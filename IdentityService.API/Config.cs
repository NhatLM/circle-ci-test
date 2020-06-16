// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityService.API
{
    public static class Config
    {
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("HowllowApi", "Howllow Service APIs")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // Later on we move client information into database for easily add/update
                new Client
                {
                    ClientId = "HollowService",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("AutoproffHowllowService".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "HowllowApi" }
                }
            };
    }
}