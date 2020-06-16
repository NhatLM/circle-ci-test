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
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (ValidateUsernamePassword(context.UserName, context.Password))
            {
                context.Result = new GrantValidationResult(subject: "818727", authenticationMethod: "custom");
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
            }

            return Task.CompletedTask;
        }

        private bool ValidateUsernamePassword(string username, string password)
        {
            bool result = true;
            string sql = "SELECT cu.cust_id FROM cust_user as cu WHERE cu.email = @username AND cu.password = PASSWORD(@password)";

            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING")))
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