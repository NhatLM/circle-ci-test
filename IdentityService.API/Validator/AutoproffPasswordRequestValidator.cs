using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using IdentityService.API.Model;
using IdentityService.API.Repository;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;

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
            context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
            //get user from db
            UserRepository userRepo = new UserRepository(new AccountsContext());
            CustUser user = userRepo.FindByUsername(context.UserName);
            if(user != null)
            {
                if(user.Password == MD5Hash(context.Password))
                {
                    context.Result = new GrantValidationResult(
                        subject: user.Email, 
                        authenticationMethod: Constants.ValidationResultMethod,
                        claims: GetClaims(user));
                }               
            }           
            return Task.CompletedTask;
        }

        private string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(Encoding.UTF8.GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public static List<Claim> GetClaims(CustUser user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            
            RoleRepository roleRepo = new RoleRepository(new AccountsContext());
            var role = roleRepo.GetRole(user.UserLevel ?? -1);
            if(role.Length > 0)
            {
                claims.Add(new Claim("role", role));               
            }
            
            return claims;
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