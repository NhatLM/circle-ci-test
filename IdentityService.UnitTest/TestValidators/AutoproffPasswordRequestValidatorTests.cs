using Xunit;
using Moq;
using IdentityService.API.Repository.Interfaces;
using IdentityService.API.Validator;
using IdentityService.API.Model;
using IdentityServer4.Validation;
using IdentityServer4.Extensions;

namespace IdentityService.UnitTest.TestValidators
{
    public class AutoproffPasswordRequestValidatorTests
    {
        private Mock<IUserRepository> _mockUserRepo;
        private AutoproffPasswordRequestValidator _validator;
        
        [Fact]
        public void ValidateAsync_InvalidAccount_ShouldNotGrantAccess()
        {
            //Arrange
            _mockUserRepo = new Mock<IUserRepository>();
            _mockUserRepo.Setup(m => m.ValidateCredentials(It.IsAny<string>(), It.IsAny<string>())).Returns((CustUser)null);

            var context = CreateContext("username", "password");

            _validator = new AutoproffPasswordRequestValidator(_mockUserRepo.Object);

            //Act
            var task = _validator.ValidateAsync(context);
            task.Wait();

            //Assert
            Assert.True(context.Result.IsError);
        }

        [Fact]
        public void ValidateAsync_ValidAccount_ShouldGrantAccess()
        {
            //Arrange
            var returnUserAfterValidate = new CustUser() { Id = 1 };
            _mockUserRepo = new Mock<IUserRepository>();
            _mockUserRepo.Setup(m => m.ValidateCredentials(It.IsAny<string>(), It.IsAny<string>())).Returns(returnUserAfterValidate);

            var context = CreateContext("username", "password");

            _validator = new AutoproffPasswordRequestValidator(_mockUserRepo.Object);

            //Act
            var task = _validator.ValidateAsync(context);
            task.Wait();

            //Assert
            Assert.False(context.Result.IsError);
            Assert.NotNull(context.Result.Subject);
            Assert.Equal(context.Result.Subject.GetSubjectId(), returnUserAfterValidate.Id.ToString());
        }



        private ResourceOwnerPasswordValidationContext CreateContext(string username, string password)
        {
            ResourceOwnerPasswordValidationContext context = new ResourceOwnerPasswordValidationContext();
            context.UserName = username;
            context.Password = password;
            return context;
        }
    }
}
