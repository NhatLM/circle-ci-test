using Xunit;
using Moq;
using IdentityService.API.Repository;
using IdentityServer4.Models;
using IdentityService.API.Services;
using IdentityService.API.Model;
using IdentityService.API.Repository.Interfaces;
using IdentityServer4.Extensions;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace IdentityService.UnitTest.TestServices
{
    public class ProfileServiceTests
    {
        Mock<IUserRepository> _repo;
        ProfileService _service;

        [Fact]
        public void GetProfileDataAsync_CantFindUser_ShouldNotSetClaim()
        {
            //Arrange
            _repo = new Mock<IUserRepository>();
            _repo.Setup(r => r.FindBytId(It.IsAny<string>())).Returns((CustUser)null);
            Claim[] claims = { new Claim("sub", "1") };
            string[] requestedClaims = { };
            ProfileDataRequestContext context = CreateProfileContext(claims, requestedClaims);
            
            _service = new ProfileService(_repo.Object);

            //Act
            var task = _service.GetProfileDataAsync(context);
            task.Wait();

            //Assert
            _repo.Verify(r => r.GetClaims(It.IsAny<CustUser>()), Times.Never);
            Assert.Empty(context.IssuedClaims);
        }

        [Fact]
        public void GetProfileDataAsync_HaveUser_ShouldSetClaimBasedOnRequestedClaimsType()
        {
            //Arrange
            Claim[] claims = { new Claim("sub", "1") };
            string[] requestedClaims = { "role", "test" };
            ProfileDataRequestContext context = CreateProfileContext(claims, requestedClaims);

            _repo = new Mock<IUserRepository>();
            _repo.Setup(r => r.FindBytId(It.IsAny<string>())).Returns(new CustUser());
            List<Claim> mockClaims = GenerateListOfClaims();
            _repo.Setup(r => r.GetClaims(It.IsAny<CustUser>())).Returns(mockClaims);
            List<Claim> expectedClaims = mockClaims.Where(c => context.RequestedClaimTypes.Contains(c.Type)).ToList();

            _service = new ProfileService(_repo.Object);
            
            //Act
            var task = _service.GetProfileDataAsync(context);
            task.Wait();

            //Assert
            _repo.Verify(r => r.GetClaims(It.IsAny<CustUser>()), Times.Once);
            Assert.Equal(expectedClaims, context.IssuedClaims);
        }

        [Fact]
        public void IsActiveAsync_ContextHaveSubClaim_IsActiveShouldBeTrue()
        {
            //Arrange
            Claim[] claims = { new Claim("sub", "1") };
            IsActiveContext context = CreateActiveContext(claims);

            _repo = new Mock<IUserRepository>();
            _service = new ProfileService(_repo.Object);
            
            //Act
            var task = _service.IsActiveAsync(context);
            task.Wait();
            
            //Assert
            Assert.True(context.IsActive);
        }

        [Fact]
        public void IsActiveAsync_ContextNotHaveSubClaim_IsActiveShouldBeFalse()
        {
            //Arrange
            Claim[] claims = {};
            IsActiveContext context = CreateActiveContext(claims);

            _repo = new Mock<IUserRepository>();
            _service = new ProfileService(_repo.Object);
            
            //Act
            var task = _service.IsActiveAsync(context);
            task.Wait();
            
            //Assert
            Assert.False(context.IsActive);
        }

        private List<Claim> GenerateListOfClaims()
        {
            List<Claim> mockClaims = new List<Claim>();
            mockClaims.Add(new Claim("role1", "test role1"));
            mockClaims.Add(new Claim("role2", "test role2"));
            mockClaims.Add(new Claim("role3", "test role2"));
            mockClaims.Add(new Claim("test", "test"));
            return mockClaims;
        }

        private ProfileDataRequestContext CreateProfileContext(Claim[] claims, string[] requestedClaims)
        {
            ProfileDataRequestContext context = new ProfileDataRequestContext();
            context.Subject = new ClaimsPrincipal(new ClaimsIdentity(claims));
            context.RequestedClaimTypes = requestedClaims;
            return context;
        }

        private IsActiveContext CreateActiveContext(Claim[] claims)
        {
            ClaimsPrincipal principle = new ClaimsPrincipal(new ClaimsIdentity(claims));
            IsActiveContext context = new IsActiveContext(principle, new Client(), "caller");
            return context;
        }
    }
}
