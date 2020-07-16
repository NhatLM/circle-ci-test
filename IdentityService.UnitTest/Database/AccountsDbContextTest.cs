using IdentityService.API.Model;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.UnitTest.Database
{
    class AccountsDbContextTest : AccountsContext
    {
        public AccountsDbContextTest() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "AccountDbTest");
        }
    }
}
