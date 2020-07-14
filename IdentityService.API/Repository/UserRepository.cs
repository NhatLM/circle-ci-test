using IdentityModel;
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

            return _context.CustUser.FirstOrDefault(c => c.Email == username);

        }
        public bool ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);
            if (user != null)
            {
                return user.Password.Equals(MD5Hash(password));
            }

            return false;
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


            var role = _roleRepository.GetRole(user.UserLevel ?? -1);
            if (role.Length > 0)
            {
                claims.Add(new Claim("role", role));
            }

            return claims;
        }

        public CustUser FindBytId(string id)
        {
            return _context.CustUser.FirstOrDefault(x => x.Id == int.Parse(id));
        }
    }
}
