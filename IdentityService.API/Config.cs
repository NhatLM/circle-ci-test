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
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource(Constants.ApiResourceName, Constants.ApiResourceDisplay)
            };

        public static IEnumerable<IdentityResource> Identities =>
            new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResource("customIdentityRes", new List<string> { "testClaim" })
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
                    AllowedScopes = { Constants.ApiResourceName, "openid", "email", "customIdentityRes" },
                    AlwaysIncludeUserClaimsInIdToken = true,
                }
            };

        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "testID",
                    Username = "test",
                    Password = "testPassword",
                    Claims = new List<Claim>
                    {
                        new Claim("testClaim", "will it work?")
                    }
                }
            };
    }
}