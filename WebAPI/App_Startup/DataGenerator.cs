using Core;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.App_Startup
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CompanyContext(
            serviceProvider.GetRequiredService<DbContextOptions<CompanyContext>>()))
            {
                // Look for any companies.
                if (context.Companies.Any())
                {
                    return;   // Data was already seeded
                }

                context.Companies.AddRange(
                    new CompanyDto
                    {
                        Id = 1,
                        Name = "Apple Inc.",
                        Exchange = "NASDAQ",
                        Ticker = "AAPL",
                        Isin = "US0378331005",
                        Website = "http://www.apple.com"
                    },
                    new CompanyDto
                    {
                        Id = 2,
                        Name = "British Airways Plc",
                        Exchange = "Pink Sheets",
                        Ticker = "BAIRY",
                        Isin = "US1104193065",
                        Website = null
                    },
                    new CompanyDto
                    {
                        Id = 3,
                        Name = "Heineken NV",
                        Exchange = "Euronext Amsterdam",
                        Ticker = "HEIA",
                        Isin = "NL0000009165",
                        Website = null
                    },
                    new CompanyDto
                    {
                        Id = 4,
                        Name = "Panasonic Corp",
                        Exchange = "Tokyo Stock Exchange",
                        Ticker = "6752",
                        Isin = "JP3866800000",
                        Website = "http://www.panasonic.co.jp"
                    },
                    new CompanyDto
                    {
                        Id = 5,
                        Name = "Porsche Automobil",
                        Exchange = "Deutsche Börse",
                        Ticker = "PAH3",
                        Isin = "DE000PAH0038",
                        Website = "https://www.porsche.com/"
                    });

                context.Users.AddRange(new UserDto
                {
                    Id= 1,
                    UserName = "clientApp",
                    Password = "angular8Pwd"
                });

                context.SaveChanges();
            }
        }
    }
}

