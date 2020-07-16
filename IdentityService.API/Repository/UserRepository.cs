using IdentityModel;
using IdentityService.API.Configuration.Constants;
using IdentityService.API.Model;
using IdentityService.API.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AccountsContext _context;
        private readonly IRoleRepository _roleRepository;
        public UserRepository(AccountsContext context, IRoleRepository roleRepository)
        {
            _context = context;
            _roleRepository = roleRepository;
        }

        public CustUser FindByUsername(string username)
        {
            var active = 1;
            return _context.CustUser.FirstOrDefault(c => c.Email == username && c.Active == active);
        }
        public CustUser ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);
            if (user != null && user.Password.Equals(MD5Hash(password)))
            {
                return user;
            }

            return null;
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
        public List<Claim> GetClaims(CustUser user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            claims.Add(new Claim(JwtClaimTypes.Name, user.Name));
            claims.Add(new Claim(RolesClaimConsts.Role, _roleRepository.GetRole(user.UserLevel ?? -1)));
            bool isAdmin = user.UserLevel < AuthorizationConsts.MinimumLevelToBeAdmin ? true : false;
            claims.Add(new Claim(RolesClaimConsts.IsAdmin, isAdmin.ToString()));
            return claims;
        }

        public CustUser FindBytId(string id)
        {
            return _context.CustUser.FirstOrDefault(x => x.Id == int.Parse(id));
        }
    }
}
