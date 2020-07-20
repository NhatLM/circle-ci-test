using Xunit;
using IdentityService.API.Repository;
using IdentityService.UnitTest.Database;
using IdentityService.API.Model;
using System.Collections.Generic;
using System;

namespace IdentityService.UnitTest.TestRepositories
{
    public class RoleRepositoryTests : IDisposable
    {
        private RoleRepository _repo;
        private AccountsDbContextTest _context;

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public void GetRoleByLevel_GiveExistUserLevel_ShouldReturnCorrectRole()
        {
            using(_context = new AccountsDbContextTest())
            {
                List<Roles> roles = new List<Roles>()
                {
                    new Roles { Name = "tester", Level = 1 },
                    new Roles { Name = "developer", Level = 2 }
                };
                
                _context.Roles.AddRange(roles);
                _context.SaveChanges();
                _repo = new RoleRepository(_context);
                var role1 = _repo.GetRoleByLevel(1);
                var role2 = _repo.GetRoleByLevel(2);
                Assert.Equal<Roles>(roles[0], role1);
                Assert.Equal<Roles>(roles[1], role2);
                _context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData(0)]
        public void GetRoleByLevel_GiveNonExistUserLevel_ShouldReturnCorrectRole(int level)
        {
            using (_context = new AccountsDbContextTest())
            {               
                _context.Roles.Add(new Roles { Name = "tester", Level = 1 });
                _context.SaveChanges();
                _repo = new RoleRepository(_context);
                var role = _repo.GetRoleByLevel(level);
                Assert.Null(role);
                _context.Database.EnsureDeleted();
            }
        }
    }
}
