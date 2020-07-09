using IdentityService.API.Model;
using System.Linq;

namespace IdentityService.API.Repository
{
    public class UserRepository
    {
        AccountsContext _context;

        public UserRepository(AccountsContext context)
        {
            _context = context;
        }

        public CustUser FindByUsername(string username)
        {
            using(_context)
            {
               return _context.CustUser.FirstOrDefault(c => c.Email == username);
            }
        }

    }
}
