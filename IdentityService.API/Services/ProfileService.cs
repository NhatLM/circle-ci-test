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
using IdentityService.API.Repository.Interfaces;
using IdentityServer4.Extensions;

namespace IdentityService.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _userRepository;
        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {

            var sub = context.Subject.GetSubjectId();

            var user = _userRepository.FindBytId(context.Subject.GetSubjectId());
            if (user != null)
            {
                var claims = _userRepository.GetClaims(user);
                claims.Add(new Claim("role", "MyRole"));
                context.IssuedClaims = claims;
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var username = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");
            if (!string.IsNullOrEmpty(username?.Value))
            {
                context.IsActive = true;
            }
        }
    }
}
