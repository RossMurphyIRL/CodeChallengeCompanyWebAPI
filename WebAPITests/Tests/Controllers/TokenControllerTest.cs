using Core;
using Core.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using WebAPI.Controllers;

namespace WebAPITests.Tests.Controllers
{
    [TestClass]
    public class TokenControllerTest:BaseTest
    {
        [TestMethod]
        public void Get_Token_User_Should_Be_Successful()
        {
            //Arrange
            var repoMock = new Mock<IUserRepository>();
            var config = new Mock<IConfiguration>();
            var tokenController = new TokenController(config.Object, repoMock.Object);
            config.Setup(x=>x["Jwt:Subject"]).Returns("CodeChallengeAccessToken");
            config.Setup(x => x["Jwt:Key"]).Returns("sdfsdfsjdbf78sdyfssdfsdfbuidfs98gdfsdbf");
            config.Setup(x => x["Jwt:Audience"]).Returns("CodeChallengenClient");
            config.Setup(x => x["Jwt:Issuer"]).Returns("CodeChallengeAuthenticationServer");
            var user = new UserDto()
            {
                UserName = "clientApp",
                Password = "angular8Pwd"
            };
            repoMock.Setup(x => x.GetUser(user.UserName,user.Password)).ReturnsAsync(user);

            //Act
            var response = tokenController.Post(user);

            //Assert
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [TestMethod]
        public void Get_Token_User_No_User_Exists_Should_Be_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<IUserRepository>();
            var config = new Mock<IConfiguration>();
            var tokenController = new TokenController(config.Object, repoMock.Object);
            var user = new UserDto()
            {
                UserName = "clientApp",
                Password = "angular8Pwd"
            };
            repoMock.Setup(x => x.GetUser(user.UserName, user.Password)).ReturnsAsync((UserDto)null);

            //Act
            var response = tokenController.Post(user);

            //Assert
            response.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            ((BadRequestObjectResult)response.Result).Value.Should().Be("Invalid credentials");

        }

        [TestMethod]
        public void Get_Token_Null_User_Should_Be_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<IUserRepository>();
            var config = new Mock<IConfiguration>();
            var tokenController = new TokenController(config.Object, repoMock.Object);
            UserDto user = null;

            //Act
            var response = tokenController.Post(user);

            //Assert
            response.Result.Should().BeOfType(typeof(BadRequestResult));
        }

        [TestMethod]
        public void Get_Token_Null_Username_Should_Be_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<IUserRepository>();
            var config = new Mock<IConfiguration>();
            var tokenController = new TokenController(config.Object, repoMock.Object);
            var user = new UserDto()
            {
                UserName = null
            };

            //Act
            var response = tokenController.Post(user);

            //Assert
            response.Result.Should().BeOfType(typeof(BadRequestResult));
        }

        [TestMethod]
        public void Get_Token_Null_Password_Should_Be_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<IUserRepository>();
            var config = new Mock<IConfiguration>();
            var tokenController = new TokenController(config.Object, repoMock.Object);
            var user = new UserDto()
            {
                UserName = "Test",
                Password = null
            };

            //Act
            var response = tokenController.Post(user);

            //Assert
            response.Result.Should().BeOfType(typeof(BadRequestResult));
        }
    }
}
