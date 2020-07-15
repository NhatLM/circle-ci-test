using IdentityService.API.Model;
using IdentityService.API.Repository.Interfaces;
using System.Linq;

namespace IdentityService.API.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AccountsContext _context;

        public RoleRepository(AccountsContext context)
        {
            _context = context;
        }

        public string GetRole(int roleLevel)
        {
            string role = "";
            role = _context.Roles.FirstOrDefault(r => r.Level == roleLevel).Name;
            return role;
        }


    }
}
