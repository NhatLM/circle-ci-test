using IdentityService.API.Model;

namespace IdentityService.UnitTest.Helper
{
    class FakedData
    {
        public static CustUser GenerateUser(string username, int? isActive, string? password)
        {
            return new CustUser
            {
                Email = username,
                Active = isActive ?? 0,
                Password = password
            };
        }
    }
}
