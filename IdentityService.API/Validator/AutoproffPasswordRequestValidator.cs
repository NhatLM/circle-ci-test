using System.Threading.Tasks;
using IdentityServer4.Validation;
using IdentityModel;
using IdentityService.API.Repository.Interfaces;

namespace IdentityService.API.Validator
{
    public class AutoproffPasswordRequestValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _userRepository;
        public AutoproffPasswordRequestValidator(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }
        /// <summary>
        /// Custom validator for grant_type = password
        /// </summary>
        /// <param name="context">Request context</param>
        /// <returns>
        /// GrantValidationResult: sucessful or contains errors if failed validation.
        /// </returns>
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = _userRepository.ValidateCredentials(context.UserName, context.Password);
            if (user != null)
            {
                context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
            }

            return Task.FromResult(0);
        }
    }
}