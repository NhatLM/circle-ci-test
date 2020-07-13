using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityService.API.Model;
using IdentityService.API.Repository;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityService.API.Validator;

namespace IdentityService.API.Service
{
    public class ProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            UserRepository userRepo = new UserRepository(new AccountsContext());
            CustUser user = null;
            if (!string.IsNullOrEmpty(context.Subject.Identity.Name))
            {
                user = userRepo.FindByUsername(context.Subject.Identity.Name);
            }
            else
            {
                var username = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                if(!string.IsNullOrEmpty(username?.Value))
                {
                    user = userRepo.FindByUsername(username.Value);                   
                }
            }
            if (user != null)
            {
                var claims = AutoproffPasswordRequestValidator.GetClaims(user);
                context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var username = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");
            if(!string.IsNullOrEmpty(username?.Value))
            {
                context.IsActive = true;
            }
        }
    }
}
