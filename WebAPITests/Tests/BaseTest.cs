using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Models;

namespace WebAPITests
{
    [TestClass]
    public class BaseTest
    {
        protected static ApiContext _context;

        public BaseTest()
        {
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "Companies_Db")
                .Options;
            _context = new ApiContext(options);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
