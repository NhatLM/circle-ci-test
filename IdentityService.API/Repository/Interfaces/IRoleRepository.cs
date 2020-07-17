using IdentityService.API.Model;

namespace IdentityService.API.Repository.Interfaces
{
    public interface IRoleRepository
    {
        Roles GetRoleByLevel(int roleLevel);
    }
}
