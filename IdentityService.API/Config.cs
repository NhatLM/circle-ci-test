// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityService.API.Validator;
using System.Collections.Generic;

namespace IdentityService.API
{
    public static class Config
    {
        // Clients and Resources can be loaded from data store. It's exactly implementation that we will use.
        // But at this point, we hard code and use in-memory version.
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource(Constants.ApiResourceName, Constants.ApiResourceDisplay)
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = Constants.ApiClient,

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret(Constants.ApiClient.Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { Constants.ApiResourceName }
                }
            };
    }
}