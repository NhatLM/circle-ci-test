using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Dapper;
using MySql.Data.MySqlClient;
using System;

namespace IdentityService.API.Validator
{
    public class AutoproffPasswordRequestValidator : IResourceOwnerPasswordValidator
    {
        /// <summary>
        /// Custom validator for grant_type = password
        /// </summary>
        /// <param name="context">Request context</param>
        /// <returns>
        /// GrantValidationResult: sucessful or contains errors if failed validation.
        /// </returns>
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            // Temporarily hard code username and password.            
            if (string.Equals(context.UserName, Constants.DefaultUsername) && string.Equals(context.Password, Constants.DefaultPassword))
            // Will be replaced with real data from autoproff database.
            // if (ValidateUsernamePassword(context.UserName, context.Password))
            {
                context.Result = new GrantValidationResult(subject: Constants.ValidationResultSubject,
                    authenticationMethod: Constants.ValidationResultMethod);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
            }

            return Task.CompletedTask;
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