using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAPITests.Helpers;

namespace WebAPITests
{
    [TestClass]
    public class BaseTest
    {
        protected static CompanyHelper _helper;
        protected static Task<List<CompanyDto>> _companies;

        public BaseTest()
        {
            _helper = new CompanyHelper();
            _companies = Task.FromResult(new List<CompanyDto>());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _companies = Task.FromResult(new List<CompanyDto>());
        }
    }

}
