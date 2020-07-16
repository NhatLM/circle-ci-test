using System.Collections.Generic;

namespace IdentityService.API.Repository.Interfaces
{
    public interface IRoleRepository
    {
        string GetRole(int roleLevel);
    }
}
