using Core;
using Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace WebAPITests.Tests.Repositories
{
    [TestClass]
    public class CompanyRepositoryTest:BaseTest
    {
        [TestMethod]
        public void Check_Company_Exists_Should_Return_True()
        {
            //Arrange
            var data = new List<CompanyDto> { _helper.CreateCompany(1, "Dell", "DOWJONES", "DJIA", "DT0123456789", "www.dell.com") };
            var mock = data.AsQueryable().BuildMockDbSet();
            var mockContext = new Mock<CompanyContext>();
            mockContext.Setup(c => c.Companies).Returns(mock.Object);
            var companyRepo = new CompanyRepository(mockContext.Object);

            //Act
            var response = companyRepo.CompanyExists(1);

            //Assert
            response.Should().Be(true);
        }

        [TestMethod]
        public void Check_Company_Exists_Should_Return_False()
        {
            //Arrange
            var data = new List<CompanyDto> { _helper.CreateCompany(1, "Dell", "DOWJONES", "DJIA", "DT0123456789", "www.dell.com") };
            var mock = data.AsQueryable().BuildMockDbSet();
            var mockContext = new Mock<CompanyContext>();
            mockContext.Setup(c => c.Companies).Returns(mock.Object);
            var companyRepo = new CompanyRepository(mockContext.Object);

            //Act
            var response = companyRepo.CompanyExists(2);

            //Assert
            response.Should().Be(false);
        }


    }
}
