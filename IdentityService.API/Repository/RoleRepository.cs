using IdentityService.API.Model;
using System.Linq;

namespace IdentityService.API.Repository
{
    public class RoleRepository
    {
        AccountsContext _context;

        public RoleRepository(AccountsContext context)
        {
            _context = context;
        }

        public string GetRole(int roleLevel)
        {
            using(_context)
            {
                string role = "";
                role = _context.Roles.FirstOrDefault(r => r.Level == roleLevel).Name;
                return role;
            }
        }
    }
}
