using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.API.Repository.Interfaces
{
    public interface IRoleRepository
    {
        string GetRole(int roleLevel);
    }
}
