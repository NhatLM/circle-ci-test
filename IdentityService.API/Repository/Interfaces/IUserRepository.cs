using IdentityService.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityService.API.Repository.Interfaces
{
    public interface IUserRepository
    {
        CustUser FindByUsername(string username);
        CustUser ValidateCredentials(string username, string password);
        List<Claim> GetClaims(CustUser user);
        CustUser  FindBytId(string id);

    }
}
