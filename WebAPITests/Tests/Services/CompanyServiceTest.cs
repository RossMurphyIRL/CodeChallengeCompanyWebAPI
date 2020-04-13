using Core;
using Core.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Services;
using Services.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPITests.Tests.Services
{
    [TestClass]
    public class CompanyServiceTest: BaseTest
    {
        [TestMethod]
        public void Validate_Isin_Should_Be_True()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyService = new CompanyService(repoMock.Object);
            var Isin = "DF5645676543";

            //Act
            var response = companyService.IsinIsValid(Isin);

            //Assert
            response.Should().Be(true);
        }

        [TestMethod]
        public void Validate_Isin_Length_Should_Be_False()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyService = new CompanyService(repoMock.Object);
            var Isin = "DF56456";

            //Act
            var response = companyService.IsinIsValid(Isin);

            //Assert
            response.Should().Be(false);
        }

        [TestMethod]
        public void Validate_Isin_Format_Should_Be_False()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyService = new CompanyService(repoMock.Object);
            var Isin = "D34567898765";

            //Act
            var response = companyService.IsinIsValid(Isin);

            //Assert
            response.Should().Be(false);
        }

        [TestMethod]
        public void Validate_Isin_Format_Null_Should_Be_False()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyService = new CompanyService(repoMock.Object);
            string Isin = null;

            //Act
            var response = companyService.IsinIsValid(Isin);

            //Assert
            response.Should().Be(false);
        }

        [TestMethod]
        public void Check_Isin_Exists_Should_Be_True()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyService = new CompanyService(repoMock.Object);
            var newCompany = _helper.AddedCompany(1, "Dell2", "DOWJONES", "DJIA", "DT9876543210", "www.dell2.com").Result;
            var savedCompany = _helper.AddedCompany(2, "Dell2", "DOWJONES", "DJIA", "DT9876543210", "www.dell2.com").Result;
            repoMock.Setup(context => context.GetCompanyByIsinWithoutCompany(newCompany)).ReturnsAsync(savedCompany);

            //Act
            var response = companyService.IsinExists(newCompany);

            //Assert
            response.Should().Be(true);
        }

        [TestMethod]
        public void Check_Isin_Exists_Null_Should_Be_False()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyService = new CompanyService(repoMock.Object);

            //Act
            var response = companyService.IsinExists(null);

            //Assert
            response.Should().Be(false);
        }

        [TestMethod]
        public void Check_Isin_Exists_Isin_Null_Should_Be_False()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyService = new CompanyService(repoMock.Object);
            var newCompany = _helper.AddedCompany(1, "Dell2", "DOWJONES", "DJIA", null, "www.dell2.com").Result;

            //Act
            var response = companyService.IsinExists(newCompany);

            //Assert
            response.Should().Be(false);
        }

        [TestMethod]
        public void Check_Isin_Exists_Should_Be_False()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyService = new CompanyService(repoMock.Object);
            var newCompany = _helper.AddedCompany(1, "Dell2", "DOWJONES", "DJIA", null, "www.dell2.com").Result;
            repoMock.Setup(context => context.GetCompanyByIsinWithoutCompany(newCompany)).ReturnsAsync((CompanyDto)null);

            //Act
            var response = companyService.IsinExists(newCompany);

            //Assert
            response.Should().Be(false);
        }

        [TestMethod]
        public void Should_Have_On_Isin_Format_And_Length_Error_On_Model()
        {
            //Arrange
            var validator = new CompanyValidator();
            var testCompany = _helper.GetCompany(1).Result;
            testCompany.Isin = "G424234";

            //Act
            var result = validator.Validate(testCompany);

            //Assert
            result.Errors.Count.Should().Be(2);
        }

        [TestMethod]
        public void Should_Have_On_Isin_Format_Error_On_Model()
        {
            //Arrange
            var validator = new CompanyValidator();
            var testCompany = _helper.GetCompany(1).Result;
            testCompany.Isin = "G42423445678";

            //Act
            var result = validator.Validate(testCompany);

            //Assert
            result.Errors.Count.Should().Be(1);
        }

        [TestMethod]
        public void Should_Have_On_Isin_Length_Error_On_Model()
        {
            //Arrange
            var validator = new CompanyValidator();
            var testCompany = _helper.GetCompany(1).Result;
            testCompany.Isin = "GL4424234";

            //Act
            var result = validator.Validate(testCompany);

            //Assert
            result.Errors.Count.Should().Be(1);
        }

        [TestMethod]
        public void Should_Have_On_Null_Properties_Error_On_Model()
        {
            //Arrange
            var validator = new CompanyValidator();
            var testCompany = _helper.GetCompany(1).Result;
            testCompany.Ticker = null;
            testCompany.Name = null;
            testCompany.Exchange = null;
            testCompany.Isin = null;

            //Act
            var result = validator.Validate(testCompany);

            //Assert
            result.Errors.Count.Should().Be(4);
        }
    }
}
