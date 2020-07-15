

using IdentityServer4.AspNetIdentity;
using IdentityService.API.Repository;
using IdentityService.API.Repository.Interfaces;
using IdentityService.API.Services;
using IdentityService.API.Validator;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CustomIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddCustomUserStore(this IIdentityServerBuilder builder)
        {
         
            builder.AddProfileService<ProfileService>();
            builder.AddResourceOwnerValidator<AutoproffPasswordRequestValidator>();

            return builder;
        }
    }
}