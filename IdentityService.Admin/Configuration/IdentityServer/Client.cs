using System.Collections.Generic;
using IdentityService.Admin.Configuration.Identity;

namespace IdentityService.Admin.Configuration.IdentityServer
{
    public class Client : global::IdentityServer4.Models.Client
    {
        public List<Claim> ClientClaims { get; set; } = new List<Claim>();
    }
}






