using Core;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebAPITests.Helpers
{
    public class CompanyHelper
    {
        public CompanyHelper()
        {
        }

        public List<CompanyDto> TestDataCompanies()
        {
            return new List<CompanyDto>() {
                    new CompanyDto
                    {
                        Id = 1,
                        Name = "Apple Inc.",
                        Exchange = "NASDAQ",
                        Ticker = "AAPL",
                        Isin = "US0378331005",
                        Website = "ApiContext"
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
                    }
                    };

        }

        public Task<CompanyDto> GetCompany(int num)
        {
            var companies = new List<CompanyDto>()
            {
                new CompanyDto
                {
                    Id = 1,
                    Name = "Apple Inc.",
                    Exchange = "NASDAQ",
                    Ticker = "AAPL",
                    Isin = "US0378331005",
                    Website = "ApiContext"
                },
                new CompanyDto
                 {
                    Id = 2,
                    Name = "British Airways Plc",
                    Exchange = "Pink Sheets",
                    Ticker = "BAIRY",
                    Isin = "US1104193065",
                    Website = null
                }
            };
            return Task.FromResult(companies[num]);
        }
        public CompanyDto CreateCompany(int id, string name, string exchange, string ticker, string isin, string website)
        {
            return new CompanyDto { Id = id, Name = name, Exchange = exchange, Ticker = ticker, Isin = isin, Website = website };
        }
        public Task<CompanyDto> AddedCompany(int id, string name, string exchange, string ticker, string isin, string website)
        {
            return Task.FromResult(new CompanyDto { Id = id, Name = name, Exchange = exchange, Ticker = ticker, Isin = isin, Website = website });
        }
    }
}
