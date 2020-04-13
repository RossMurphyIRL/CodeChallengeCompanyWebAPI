using Core;
using Core.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Controllers;
using static Services.CustomExceptions.CompanyExceptions;

namespace WebAPITests.Tests.Controllers
{
    [TestClass]
    public class CompaniesControllerTest : BaseTest
    {

        /* ----------Successful indicates Http Status Code 2xx Success---------- */
        /* ----------Client_Error indicates Http Status Code 4xx Client Error---------- */

        /* ----------GetCompanies ---------- */
        [TestMethod]
        public void Get_All_Companies_With_No_Added_Data_Should_Be_Successful()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            repoMock.Setup(context => context.GetCompanies()).Returns(_companies);
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);

            //Act
            var response = controller.GetCompanies();

            //Assert
            response.Exception.Should().BeNull("Problem getting list of companies");
            var data = response.Result.Value.ToList();
            data.Count.Should().Be(0);
        }

        [TestMethod]
        public void Get_All_Companies_With_Added_Data_Should_Be_Successful()
        {
            //Arrange
            _companies = Task.FromResult(_helper.TestDataCompanies());
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            repoMock.Setup(context => context.GetCompanies()).Returns(_companies);
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);

            //Act
            var response = controller.GetCompanies();

            //Assert
            response.Exception.Should().BeNull("Problem getting list of companies");
            response.Status.Should().Be(TaskStatus.RanToCompletion);
            var data = response.Result.Value.ToList();
            data.Count.Should().Be(5);
        }

        /* ----------GetCompany() ---------- */
        [TestMethod]
        public void Get_CompanyById_Should_Be_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            repoMock.Setup(context => context.GetCompanies()).Returns(_companies);
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);

            //Act
            var response = controller.GetCompany(0);

            //Assert
            response.Result.Result.Should().BeOfType(typeof(NotFoundResult));
        }

        [TestMethod]
        public void Get_CompanyById_Should_Be_Successful()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var companyId = 1;
            repoMock.Setup(context => context.GetCompany(companyId)).Returns(_helper.GetCompany(0));

            //Act
            var response = controller.GetCompany(companyId);

            //Assert
            response.Exception.Should().BeNull("Problem getting company by id " + companyId);
            var data = response.Result.Value as CompanyDto;
            response.Status.Should().Be(TaskStatus.RanToCompletion);
            data.Id.Should().Be(companyId);
            data.Name.Should().Be("Apple Inc.");
            data.Exchange.Should().Be("NASDAQ");
            data.Ticker.Should().Be("AAPL");
            data.Isin.Should().Be("US0378331005");
            data.Website.Should().Be("ApiContext");
        }

        /* ----------GetCompanyByIsin() ---------- */
        [TestMethod]
        public void Get_CompanyByIsin_Should_Be_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var isin = "IO6546756234";
            companyServiceMock.Setup(context => context.IsinIsValid(isin)).Returns(false);

            //Act
            var response = controller.GetCompanyByIsin(isin);

            //Assert
            response.Result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            ((BadRequestObjectResult)response.Result.Result).Value.Should().Be("Requested isin must be in correct format.");
        }

        [TestMethod]
        public void Get_CompanyByIsin_With_Company_Returned_Should_Be_Successful()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var isin = "US1104193065";
            var companyId = 2;
            companyServiceMock.Setup(context => context.IsinIsValid(isin)).Returns(true);
            repoMock.Setup(context => context.GetCompanyByIsin(isin)).Returns(_helper.GetCompany(1));

            //Act
            var response = controller.GetCompanyByIsin(isin);

            //Assert
            response.Exception.Should().BeNull("Problem getting company by Isin " + isin);
            response.Status.Should().Be(TaskStatus.RanToCompletion);
            var data = response.Result.Value as CompanyDto;
            data.Id.Should().Be(companyId);
            data.Name.Should().Be("British Airways Plc");
            data.Exchange.Should().Be("Pink Sheets");
            data.Ticker.Should().Be("BAIRY");
            data.Isin.Should().Be(isin);
            data.Website.Should().BeNull();
        }

        [TestMethod]
        public void Get_CompanyByIsin_With_Incorrect_Prefix_Should_Be_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var isin = "556546756234";
            companyServiceMock.Setup(context => context.IsinIsValid(isin)).Returns(false);

            //Act
            var response = controller.GetCompanyByIsin(isin);

            //Assert
            response.Result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            ((BadRequestObjectResult)response.Result.Result).Value.Should().Be("Requested isin must be in correct format.");
        }

        [TestMethod]
        public void Get_CompanyByIsin_With_Incorrect_Isin_Length_Should_Be_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var isin = "MS6766";
            companyServiceMock.Setup(context => context.IsinIsValid(isin)).Returns(false);

            //Act
            var response = controller.GetCompanyByIsin(isin);

            //Assert
            response.Result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            ((BadRequestObjectResult)response.Result.Result).Value.Should().Be("Requested isin must be in correct format.");
        }

        /* ----------AddCompany() ---------- */
        [TestMethod]
        public void Add_Company_Record_Should_Be_Successful()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var newCompany = _helper.CreateCompany(0, "Dell", "DOWJONES", "DJIA", "DT0123456789", "www.dell.com");
            var savedCompany = _helper.AddedCompany(1, "Dell", "DOWJONES", "DJIA", "DT0123456789", "www.dell.com");
            repoMock.Setup(context => context.Add(newCompany)).Returns(savedCompany);

            //Act
            var response = controller.PostCompany(newCompany);

            //Assert
            var data = ((CreatedAtActionResult)response.Result.Result).Value as CompanyDto;
            response.Status.Should().Be(TaskStatus.RanToCompletion);
            data.Id.Should().Be(1);
            data.Name.Should().Be("Dell");
            data.Exchange.Should().Be("DOWJONES");
            data.Ticker.Should().Be("DJIA");
            data.Isin.Should().Be("DT0123456789");
            data.Website.Should().Be("www.dell.com");
        }

        [TestMethod]
        public void Add_Company_Record_With_Preveous_Data_Should_Successful()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var newCompany = _helper.CreateCompany(0, "Dell2", "DOWJONES", "DJIA", "AI5555554321", "www.dell2.com");
            var savedCompany = _helper.AddedCompany(6, "Dell2", "DOWJONES", "DJIA", "AI5555554321", "www.dell2.com");
            repoMock.Setup(x=>x.Add(newCompany)).Returns(savedCompany);

            //Act
            var response = controller.PostCompany(newCompany);

            //Assert
            var data = ((CreatedAtActionResult)response.Result.Result).Value as CompanyDto;
            data.Id.Should().Be(6);
            data.Name.Should().Be("Dell2");
            data.Exchange.Should().Be("DOWJONES");
            data.Ticker.Should().Be("DJIA");
            data.Isin.Should().Be("AI5555554321");
            data.Website.Should().Be("www.dell2.com");
        }

        [TestMethod]
        public void Add_Company_Record_With_Duplicate_Isin_Should_Be_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var savedCompany = _helper.AddedCompany(1, "Dell2", "DOWJONES", "DJIA", "DT9876543210", "www.dell2.com").Result;
            companyServiceMock.Setup(x=>x.IsinExists(savedCompany)).Returns(true);

            //Act
            var response = controller.PostCompany(savedCompany);

            //Assert
            response.Result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            ((BadRequestObjectResult)response.Result.Result).Value.Should().Be("Company with isin: " + savedCompany.Isin + " already exists.");
        }


        /* ----------UpdateCompany() ---------- */
        [TestMethod]
        public void Update_Company_Record_Should_Be_Successful()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var savedCompany = _helper.AddedCompany(2, "Dell2", "DOWJONES", "DJIA", "DT9876543210", "www.dell2.com").Result;
            repoMock.Setup(context => context.UpdateCompany(savedCompany)).ReturnsAsync(1);

            //Act
            var response = controller.PutCompany(savedCompany.Id, savedCompany);

            //Assert
            response.Result.Should().BeOfType(typeof(NoContentResult));
        }

        [TestMethod]
        public void Update_Company_Record_With_Invalid_Model_Should_Be_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var savedCompany = _helper.AddedCompany(0, "Dell2", "DOWJONES", "DJIA", "DT9876543210", "www.dell2.com").Result;

            //Act
            var response = controller.PutCompany(savedCompany.Id, savedCompany);

            //Assert
            response.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            ((BadRequestObjectResult)response.Result).Value.Should().Be("Company Id not valid");

        }

        [TestMethod]
        public void Update_Company_Record_With_Invalid_Isin_Should_Be_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var savedCompany = _helper.AddedCompany(5, "Dell", "DOWJONES", "DJIA", "JP3866800000", "www.dell.com").Result;

            //Act
             var response = controller.PutCompany(savedCompany.Id, savedCompany);

            //Assert
            response.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            ((BadRequestObjectResult)response.Result).Value.Should().Be("Error updating company");
        }

        [TestMethod]
        public void Update_Company_Record_Should_Throw_Error_Duplicate_Isin()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var savedCompany = _helper.AddedCompany(5, "Dell", "DOWJONES", "DJIA", "JP3866800000", "www.dell.com").Result;
            companyServiceMock.Setup(x => x.IsinExists(savedCompany)).Returns(true);

            //Act
            var response = controller.PutCompany(savedCompany.Id, savedCompany);

            //Assert
            response.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            ((BadRequestObjectResult)response.Result).Value.Should().Be("Company with isin: " + savedCompany.Isin + " already exists.");
        }

        [TestMethod]
        public void Update_Company_Record_Id_And_CompanyId_Not_Matching_Should_Be_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var incorrectId = 999;
            var savedCompany = _helper.AddedCompany(5, "Dell", "DOWJONES", "DJIA", "DT0123456789", "www.dell.com");


            //Act
            var response = controller.PutCompany(incorrectId, savedCompany.Result);

            //Assert
            response.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            ((BadRequestObjectResult)response.Result).Value.Should().Be("Company Id not valid");
        }


        /* ----------DeleteCompany() ---------- */
        [TestMethod]
        public void Delete_Company_Record_By_Id_Should_Be_Successful()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var companyId = 5;
            var savedCompany = _helper.AddedCompany(5, "Dell", "DOWJONES", "DJIA", "DT0123456789", "www.dell.com");
            repoMock.Setup(context => context.GetCompany(companyId)).Returns(savedCompany);
            repoMock.Setup(context => context.Delete(savedCompany.Result));

            //Act
            var response = controller.DeleteCompany(companyId);

            //Assert
            response.Exception.Should().BeNull("Problem deleting company record by id " + companyId);
            response.Status.Should().Be(TaskStatus.RanToCompletion);
            var data = response.Result.Value as CompanyDto;
            data.Id.Should().Be(companyId);
        }

        [TestMethod]
        public void Delete_Company_Record_By_Id_Should_Pass_Client_Error()
        {
            //Arrange
            var repoMock = new Mock<ICompanyContext>();
            var companyServiceMock = new Mock<ICompanyService>();
            var controller = new CompaniesController(repoMock.Object, companyServiceMock.Object);
            var companyId = 8;
            repoMock.Setup(context => context.GetCompany(companyId)).ReturnsAsync((CompanyDto)null);

            //Act
            var response = controller.DeleteCompany(companyId);

            //Assert
            response.Result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            ((NotFoundObjectResult)response.Result.Result).Value.Should().Be("Can not find company");
        }
    }
}
