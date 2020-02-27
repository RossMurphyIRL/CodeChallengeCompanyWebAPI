using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Controllers;
using WebAPI.Models;
using WebAPITests.Helpers;
using static WebAPI.CustomExceptions.CustomExceptions;

namespace WebAPITests
{
    [TestClass]
    public class CompanyApiTest: BaseTest
    {
        /* ----------GetCompanies ---------- */
        [TestMethod]
        public void Get_All_Companies_With_No_Added_Data_Should_Pass()
        {
            //Arrange
            var controller = new CompaniesController(_context);

            //Act
            var response = controller.GetCompanies();

            //Assert
            response.Exception.Should().BeNull("Problem getting list of companies");
            var data = response.Result.Value.ToList();
            data.Count.Should().Be(0);
        }

        [TestMethod]
        public void Get_All_Companies_With_Added_Data_Should_Pass()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);

            //Act
            helper.AddCompanies();
            var response = controller.GetCompanies();

            //Assert
            response.Exception.Should().BeNull("Problem getting list of companies");
            var data = response.Result.Value.ToList();
            data.Count.Should().Be(5);
        }


        /* ----------GetCompany() ---------- */
        [TestMethod]
        public void Get_CompanyById_Should_Pass_With_NotFound()
        {
            //Arrange
            var controller = new CompaniesController(_context);

            //Act
            var response = controller.GetCompany(0);

            //Assert
            response.Result.Result.Should().BeOfType(typeof(NotFoundResult));
        }

        [TestMethod]
        public void Get_CompanyById_Should_Pass_With_Company_Returned()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);
            var companyId = 1;

            //Act
            helper.AddCompanies();
            var response = controller.GetCompany(companyId);

            //Assert
            response.Exception.Should().BeNull("Problem getting company by id " + companyId);
            var data = response.Result.Value as Company;
            data.Id.Should().Be(companyId);
            data.Name.Should().Be("Apple Inc.");
            data.Exchange.Should().Be("NASDAQ");
            data.Ticker.Should().Be("AAPL");
            data.Isin.Should().Be("US0378331005");
            data.Website.Should().Be("ApiContext");
        }

        /* ----------GetCompanyByIsin() ---------- */
        [TestMethod]
        public void Get_CompanyByIsin_Should_Pass_With_NotFound()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var isin = "IO6546756234";

            //Act
            var response = controller.GetCompanyByIsin(isin);

            //Assert
            response.Result.Result.Should().BeOfType(typeof(NotFoundResult));
        }

        [TestMethod]
        public void Get_CompanyByIsin_Should_Pass_With_Company_Returned()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);
            var isin = "US1104193065";
            var companyId = 2;

            //Act
            helper.AddCompanies();
            var response = controller.GetCompanyByIsin(isin);

            //Assert
            response.Exception.Should().BeNull("Problem getting company by Isin " + isin);
            var data = response.Result.Value as Company;
            data.Id.Should().Be(companyId);
            data.Name.Should().Be("British Airways Plc");
            data.Exchange.Should().Be("Pink Sheets");
            data.Ticker.Should().Be("BAIRY");
            data.Isin.Should().Be(isin);
            data.Website.Should().BeNull();
        }

        [TestMethod]
        public void Get_CompanyByIsin_Should_Throw_Exception_With_Incorrect_Prefix()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var isin = "556546756234";

            //Act
            var response = controller.GetCompanyByIsin(isin);

            //Assert
            response.Exception.Should().NotBeNull("Requested isin wasn't in correct format, should have thrown error.");
            response.Exception.Message.Should().Contain("Requested isin must be in correct format.");
        }

        [TestMethod]
        public void Get_CompanyByIsin_Should_Throw_Exception_With_Incorrect_Isin_Length()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var isin = "MS6766";

            //Act
            var response = controller.GetCompanyByIsin(isin);

            //Assert
            response.Exception.Should().NotBeNull("Requested isin wasn't in correct format, should have thrown error.");
            response.Exception.Message.Should().Contain("Requested isin must be in correct format.");
        }

        /* ----------AddCompany() ---------- */
        [TestMethod]
        public void Add_Company_Record_Should_Pass()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);
            var newCompany = helper.CreateCompany(0, "Dell", "DOWJONES", "DJIA", "DT0123456789", "www.dell.com");

            //Act
            var response = controller.PostCompany(newCompany);

            //Assert
            var data = ((CreatedAtActionResult)response.Result.Result).Value as Company;
            data.Id.Should().Be(1);
            data.Name.Should().Be("Dell");
            data.Exchange.Should().Be("DOWJONES");
            data.Ticker.Should().Be("DJIA");
            data.Isin.Should().Be("DT0123456789");
            data.Website.Should().Be("www.dell.com");
        }

        [TestMethod]
        public void Add_Company_Record_With_Prevuous_Data_Should_Pass()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);
            var newCompany = helper.CreateCompany(0, "Dell2", "DOWJONES", "DJIA", "AI5555554321", "www.dell2.com");

            //Act
            helper.AddCompanies();
            var companyCountBefore = _context.Companies.Count();
            var response = controller.PostCompany(newCompany);

            //Assert
            _context.Companies.Count().Should().Be(companyCountBefore + 1);
            var data = ((CreatedAtActionResult)response.Result.Result).Value as Company;
            data.Id.Should().Be(6);
            data.Name.Should().Be("Dell2");
            data.Exchange.Should().Be("DOWJONES");
            data.Ticker.Should().Be("DJIA");
            data.Isin.Should().Be("AI5555554321");
            data.Website.Should().Be("www.dell2.com");
        }

        [TestMethod]
        public void Add_Company_Record_Should_Throw_Error_Invalid_Model()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);
            var newCompany = helper.CreateCompany(0, "Dell2", "DOWJONES", "DJIA", "DT9876543210", "www.dell2.com");

            //Act
            controller.ModelState.AddModelError("test", "test");
            helper.AddCompanies();

            var response = controller.PostCompany(newCompany);

            //Assert
            response.Exception.Should().NotBeNull("Invalid model state should have thrown error.");
            response.Exception.Message.Should().Contain("Company model is in invalid state.");
        }

        [TestMethod]
        public void Add_Company_Record_Should_Throw_Error_Invalid_Isin()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);
            var newCompany = helper.CreateCompany(0, "Dell2", "DOWJONES", "DJIA", "543210", "www.dell2.com");
            newCompany.Exchange = null;

            //Act
            helper.AddCompanies();
            var response = controller.PostCompany(newCompany);

            //Assert
            response.Exception.Should().NotBeNull("Requested isin wasn't in correct format, should have thrown error.");
            response.Exception.Message.Should().Contain("Add Isin value must be in correct format.");
        }

        [TestMethod]
        public void Add_Company_Record_Should_Throw_Error_Duplicate_Isin()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);
            var newCompany = helper.CreateCompany(0, "Dell2", "DOWJONES", "DJIA", "JP3866800000", "www.dell2.com");
            newCompany.Exchange = null;

            //Act
            helper.AddCompanies();
            var response = controller.PostCompany(newCompany);

            //Assert
            response.Exception.Should().NotBeNull("Requested isin is duplicate, should have thrown error.");
            response.Exception.Message.Should().Contain("Company with isin:" + newCompany.Isin + " already exists.");
        }


        /* ----------UpdateCompany() ---------- */
        [TestMethod]
        public void Update_Company_Record_Should_Pass()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);

            //Act
            helper.AddCompanies();
            var company = _context.Companies.First();
            company.Exchange = "Aztec";
            company.Ticker = "Ball";
            company.Isin = "BL7654398765";
            var response = controller.PutCompany(company.Id, company);

            //Assert
            response.Result.Should().BeOfType(typeof(NoContentResult));
            var updateCompany = _context.Companies.Single(x=>x.Id == company.Id);
            updateCompany.Exchange.Should().Be("Aztec");
            updateCompany.Ticker.Should().Be("Ball");
        }

        [TestMethod]
        public void Update_Company_Record_Should_Throw_Error_Invalid_Model()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);
            var newCompany = helper.CreateCompany(0, "Dell2", "DOWJONES", "DJIA", "DT9876543210", "www.dell2.com");

            //Act
            controller.ModelState.AddModelError("test", "test");

            var response = controller.PutCompany(newCompany.Id,newCompany);

            //Assert
            response.Exception.Should().NotBeNull("Invalid model state should have thrown error.");
            response.Exception.Message.Should().Contain("Company model is in invalid state.");
        }

        [TestMethod]
        public void Update_Company_Record_Should_Throw_Error_Invalid_Isin()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);

            //Act
            helper.AddCompanies();
            var updateCompany = _context.Companies.First();
            updateCompany.Isin = "1A45345435";

            var response = controller.PutCompany(updateCompany.Id,updateCompany);

            //Assert
            response.Exception.Should().NotBeNull("Requested isin wasn't in correct format, should have thrown error.");
            response.Exception.Message.Should().Contain("Update Isin value must be in correct format.");
        }

        [TestMethod]
        public void Update_Company_Record_Should_Throw_Error_Duplicate_Isin()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);

            //Act
            helper.AddCompanies();
            var updateCompany = _context.Companies.First();
            updateCompany.Isin = "JP3866800000";
            var response = controller.PutCompany(updateCompany.Id, updateCompany);

            //Assert
            response.Exception.Should().NotBeNull("Requested isin is duplicate, should have thrown error.");
            response.Exception.Message.Should().Contain("Company with isin:" + updateCompany.Isin + " already exists.");
        }

        [TestMethod]
        public void Update_Company_Record_Should_Throw_Error_Id_And_CompanyId_Not_Matching()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);
            var incorrectId = 999; 

            //Act
            helper.AddCompanies();
            var company = _context.Companies.First();
            company.Exchange = "Aztec";
            company.Ticker = "Ball";
            var response = controller.PutCompany(incorrectId, company);

            //Assert
            response.Result.Should().BeOfType(typeof(BadRequestResult));
        }


        /* ----------DeleteCompany() ---------- */
        [TestMethod]
        public void Delete_Company_Record_By_Id_Should_Pass()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);
            var companyId = 5;

            //Act
            helper.AddCompanies();
            var companyCountBefore = _context.Companies.Count();
            var response = controller.DeleteCompany(companyId);

            //Assert
            response.Exception.Should().BeNull("Problem deleting company record by id " + companyId);
            var data = response.Result.Value as Company;
            data.Id.Should().Be(companyId);
            _context.Companies.Count().Should().Be(companyCountBefore - 1);
        }

        [TestMethod]
        public void Delete_Company_Record_By_Id_Should_Pass_With_NotFound()
        {
            //Arrange
            var controller = new CompaniesController(_context);
            var helper = new CompanyHelper(_context);
            var companyId = 8;

            //Act
            helper.AddCompanies();
            var response = controller.DeleteCompany(companyId);

            //Assert
            response.Result.Result.Should().BeOfType(typeof(NotFoundResult));
        }

    }
}
