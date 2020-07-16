using System.Threading.Tasks;
using IdentityServer4.Validation;
using Dapper;
using MySql.Data.MySqlClient;
using System;
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

        /// <summary>
        /// Validate username and password.
        /// </summary>
        /// <param name="username">Login email</param>
        /// <param name="password">Login password</param>
        /// <returns>
        /// bool: validate successful = true, otherwise = false
        /// </returns>
        private bool ValidateUsernamePassword(string username, string password)
        {
            bool result = true;
            string sql = "SELECT cu.cust_id FROM cust_user as cu WHERE cu.email = @username AND cu.password = PASSWORD(@password)";

            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable(Constants.EnvNameConString)))
            {
                string userId = connection.ExecuteScalar<string>(sql, new { username = username, password = password });
                if (string.IsNullOrEmpty(userId))
                {
                    result = false;
                }
            }
            return result;
        }
    }
}