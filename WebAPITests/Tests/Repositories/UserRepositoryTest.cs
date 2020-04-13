using Core;
using FluentAssertions;
using Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPITests.Helpers;

namespace WebAPITests.Tests.Repositories
{
    [TestClass]
    public class UserRepositoryTest: BaseTest
    {
        [TestMethod]
        public async Task Get_User_With_Correct_Username_And_Password_Should_Return_User()
        {
            //Arrange
            var data = new List<UserDto>{ UserHelper._user };
            var mock = data.AsQueryable().BuildMockDbSet();
            var mockContext = new Mock<CompanyContext>();
            mockContext.Setup(c => c.Users).Returns(mock.Object);
            var userRepo = new UserRepository(mockContext.Object);

            //Act
            var response = await userRepo.GetUser(UserHelper._user.UserName, UserHelper._user.Password);

            //Assert
            response.UserName.Should().Be(UserHelper._user.UserName);
            response.Password.Should().Be(UserHelper._user.Password);
        }

        [TestMethod]
        public async Task Get_User_With_Correct_Username_And_Password_Multiple_Users_In_Db_Should_Return_User()
        {
            //Arrange
            var data = new List<UserDto> { UserHelper._user, UserHelper._user };
            var mock = data.AsQueryable().BuildMockDbSet();
            var mockContext = new Mock<CompanyContext>();
            mockContext.Setup(c => c.Users).Returns(mock.Object);
            var userRepo = new UserRepository(mockContext.Object);

            //Act
            var response = await userRepo.GetUser(UserHelper._user.UserName, UserHelper._user.Password);

            //Assert
            response.UserName.Should().Be(UserHelper._user.UserName);
            response.Password.Should().Be(UserHelper._user.Password);
        }

        [TestMethod]
        public async Task Get_User_With_No_Correct_Username_And_Password_Should_Return_Null()
        {
            //Arrange
            var data = new List<UserDto>();
            var mock = data.AsQueryable().BuildMockDbSet();
            var mockContext = new Mock<CompanyContext>();
            mockContext.Setup(c => c.Users).Returns(mock.Object);
            var userRepo = new UserRepository(mockContext.Object);

            //Act
            var response = await userRepo.GetUser(UserHelper._user.UserName, UserHelper._user.Password);

            //Assert
            response.Should().BeNull();
        }
    }
}
