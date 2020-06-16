using System;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace IdentityService.API.Validator
{
    public class AutoproffPasswordRequestValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.UserName == "bao_dep_trai" && context.Password == "deocopass")
            {
                context.Result = new GrantValidationResult(subject: "818727", authenticationMethod: "custom");
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
            }

            return Task.CompletedTask;
        }
    }
}