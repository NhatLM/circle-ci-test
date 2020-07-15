// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityService.API.Validator;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityService.API
{
    public static class Config
    {
        // Clients and Resources can be loaded from data store. It's exactly implementation that we will use.
        // But at this point, we hard code and use in-memory version.
        public static IEnumerable<IdentityResource> Identities =>
            new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResource("role_res", new List<string> { "role" })
            };
        

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = Constants.ApiClient,

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = { GrantType.ResourceOwnerPassword, GrantType.ClientCredentials },

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret(Constants.ApiClient.Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "openid", "email", "role_res" },
                    AlwaysIncludeUserClaimsInIdToken = true,
                }
            };
    }
}