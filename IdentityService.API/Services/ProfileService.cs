using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;
using System.Linq;
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
                context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
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
