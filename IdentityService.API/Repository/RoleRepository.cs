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

        public Roles GetRoleByLevel(int roleLevel)
        {
            return _context.Roles.FirstOrDefault(r => r.Level == roleLevel);
        }


    }
}
