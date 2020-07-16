using Xunit;
using IdentityService.UnitTest.Database;
using IdentityService.API.Repository;
using IdentityService.API.Repository.Interfaces;
using IdentityService.API.Model;
using Moq;
using IdentityService.UnitTest.Helper;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using IdentityService.API.Configuration.Constants;
using System;

namespace IdentityService.UnitTest.TestRepositories
{
    public class UserRepositoryTests : IDisposable
    {
        private UserRepository _repo;
        private Mock<IRoleRepository> _mockRoleRepo;
        private AccountsDbContextTest _context;
        private const int ACTIVE_ACCOUNT = 1;
        private const int INACTIVE_ACCOUNT = 0;
        public UserRepositoryTests()
        {
            _mockRoleRepo = new Mock<IRoleRepository>();
            if(_context == null)
            {
                _context = new AccountsDbContextTest();
            }
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("username")]
        public void FindByUsername_GiveCorrectUsernameAndAccountIsNotActive_ShouldReturnNull(string username)
        {
            using (_context = new AccountsDbContextTest())
            {
                _context.Database.EnsureDeleted();
                _context.CustUser.Add(FakedData.GenerateUser(username, INACTIVE_ACCOUNT, null));
                _context.SaveChanges();
                
                _repo = new UserRepository(_context, _mockRoleRepo.Object);
                var returnUser = _repo.FindByUsername(username);
                Assert.Null(returnUser);
            }
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("username")]
        public void FindByUsername_GiveCorrectUsernameAndAccountIsActive_ShouldReturnCorrectUser(string username)
        {
            using (_context = new AccountsDbContextTest())
            {
                _context.Database.EnsureDeleted();
                var user = FakedData.GenerateUser(username, ACTIVE_ACCOUNT, null);
                _context.CustUser.Add(user);
                _context.SaveChanges();

                _repo = new UserRepository(_context, _mockRoleRepo.Object);
                var returnUser = _repo.FindByUsername(username);
                Assert.NotNull(returnUser);
                Assert.Equal<CustUser>(user, returnUser);
            }
        }

        [Theory]
        [InlineData("123")]
        [InlineData(null)]
        public void FindByUsername_GiveNonExistUsernameAndAccountIsActive_ShouldReturnNull(string givenUsername)
        {
            using ( _context = new AccountsDbContextTest())
            {
                _context.Database.EnsureDeleted();
                //Add a user to repo
                var fixedUsername = "test username";
                _context.CustUser.Add(FakedData.GenerateUser(fixedUsername, ACTIVE_ACCOUNT, null));
                _context.SaveChanges();

                _repo = new UserRepository(_context, _mockRoleRepo.Object);
                var returnUser = _repo.FindByUsername(givenUsername);
                Assert.Null(returnUser);
            }
        }

        [Theory]
        [InlineData("123@///[.]")]
        [InlineData("")]
        [InlineData(null)]
        public void FindByUsername_GiveNonExistUsernameAndAccountIsNotActive_ShouldReturnNull(string givenUsername)
        {
            using ( _context = new AccountsDbContextTest())
            {
                _context.Database.EnsureDeleted();
                //Add a user to repo
                var fixedUsername = "test username";
                _context.CustUser.Add(FakedData.GenerateUser(fixedUsername, INACTIVE_ACCOUNT, null));
                _context.SaveChanges();

                _repo = new UserRepository(_context, _mockRoleRepo.Object);
                var returnUser = _repo.FindByUsername(givenUsername);
                Assert.Null(returnUser);
            }
        }

        [Fact]
        public void ValidateCredentials_GiveCorrectAccount_ShouldReturnUser()
        {
            using ( _context = new AccountsDbContextTest())
            {
                _context.Database.EnsureDeleted();
                //Add a user to repo
                var username = "test@mail.com";
                var password = "password";
                var encryptedPassword = Utilities.MD5Hash(password);
                var user = FakedData.GenerateUser(username, ACTIVE_ACCOUNT, encryptedPassword);
                _context.CustUser.Add(user);
                _context.SaveChanges();

                _repo = new UserRepository(_context, _mockRoleRepo.Object);
                var result = _repo.ValidateCredentials(username, password);

                Assert.NotNull(result);
                Assert.Equal<CustUser>(user, result);
            }
        }

        [Theory]
        [InlineData("test","")]
        [InlineData("","")]
        [InlineData("","123123/@#$")]
        [InlineData(null, null)]
        public void ValidateCredentials_GiveIncorrectAccount_ShouldReturnNull(string username, string password)
        {
            using ( _context = new AccountsDbContextTest())
            {
                _context.Database.EnsureDeleted();
                //Add a user to repo
                var usernameDb = "test@mail.com";
                var passwordDb = "password";
                var encryptedPassword = Utilities.MD5Hash(passwordDb);
                var user = FakedData.GenerateUser(usernameDb, ACTIVE_ACCOUNT, encryptedPassword);
                _context.CustUser.Add(user);
                _context.SaveChanges();

                _repo = new UserRepository(_context, _mockRoleRepo.Object);
                var result = _repo.ValidateCredentials(username, password);

                Assert.Null(result);
            }
        }
        //getClaim(user);
        [Theory]
        [InlineData(150)]
        [InlineData(200)]
        public void GetClaims_DontHaveUserLevelForAdmin_ReturnIsAdminEqualsFalse(int userlevel)
        {
            using ( _context = new AccountsDbContextTest())
            {
                CustUser user = new CustUser();
                user.UserLevel = userlevel;
                _mockRoleRepo.Setup(r => r.GetRoleByLevel(It.IsAny<int>())).Returns(new Roles());
                _context.Database.EnsureDeleted();
                _repo = new UserRepository(_context, _mockRoleRepo.Object);
                List<Claim> claims = _repo.GetClaims(user);
                Claim claim = claims.FirstOrDefault(c => c.Type == RolesClaimConsts.IsAdmin);
                Assert.NotNull(claim);
                Assert.False(bool.Parse(claim.Value));
                _mockRoleRepo.Verify(r => r.GetRoleByLevel(It.IsAny<int>()), Times.AtLeastOnce);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(149)]
        public void GetClaims_HaveUserLevelForAdmin_ReturnIsAdminEqualsTrue(int userlevel)
        {
            using ( _context = new AccountsDbContextTest())
            {
                CustUser user = new CustUser();
                user.UserLevel = userlevel;
                _mockRoleRepo.Setup(r => r.GetRoleByLevel(It.IsAny<int>())).Returns((Roles)null);
                _context.Database.EnsureDeleted();
                _repo = new UserRepository(_context, _mockRoleRepo.Object);
                List<Claim> claims = _repo.GetClaims(user);
                Claim claim = claims.FirstOrDefault(c => c.Type == RolesClaimConsts.IsAdmin);
                Assert.NotNull(claim);
                Assert.True(bool.Parse(claim.Value));
                _mockRoleRepo.Verify(r => r.GetRoleByLevel(It.IsAny<int>()), Times.AtLeastOnce);
            }
        }

        [Fact]
        public void GetClaims_NotHaveUserLevel_ShouldHaveEmptyValueInRoleClaim()
        {
            using ( _context = new AccountsDbContextTest())
            {
                CustUser user = new CustUser();
                user.UserLevel = null;
                _mockRoleRepo.Setup(r => r.GetRoleByLevel(It.IsAny<int>())).Returns((Roles)null);
                _context.Database.EnsureDeleted();
                _repo = new UserRepository(_context, _mockRoleRepo.Object);
                List<Claim> claims = _repo.GetClaims(user);
                Claim claim = claims.FirstOrDefault(c => c.Type == RolesClaimConsts.Role);
                Assert.NotNull(claim);
                Assert.Empty(claim.Value);
                _mockRoleRepo.Verify(r => r.GetRoleByLevel(It.IsAny<int>()), Times.AtLeastOnce);
            }
        }
   
        [Fact]
        public void FindById_GiveCorrectID_ShouldReturnCorrectUser()
        {
            using (_context = new AccountsDbContextTest())
            {
                var user = FakedData.GenerateUser("test", INACTIVE_ACCOUNT, null);
                var userEntity = _context.CustUser.Add(user);
                _context.SaveChanges();

                _repo = new UserRepository(_context, _mockRoleRepo.Object);

                var returnUser = _repo.FindBytId(userEntity.Entity.Id.ToString());
                Assert.NotNull(returnUser);
                Assert.Equal<CustUser>(user, returnUser);
            }
        }

        [Fact]
        public void FindById_GiveIncorrectID_ShouldDifferentUser()
        {
            using ( _context = new AccountsDbContextTest())
            {
                _context.Database.EnsureDeleted();
                var user1 = FakedData.GenerateUser("test", INACTIVE_ACCOUNT, null);
                var user2 = FakedData.GenerateUser("test2", INACTIVE_ACCOUNT, null);
                var userEntity1 = _context.CustUser.Add(user1);
                var userEntity2 = _context.CustUser.Add(user2);
                _context.SaveChanges();

                _repo = new UserRepository(_context, _mockRoleRepo.Object);

                var returnUser = _repo.FindBytId(userEntity1.Entity.Id.ToString());
                Assert.NotNull(returnUser);
                Assert.NotEqual<CustUser>(user2, returnUser);

                var NotExistUser = _repo.FindBytId(int.MaxValue.ToString());
                Assert.Null(NotExistUser);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
