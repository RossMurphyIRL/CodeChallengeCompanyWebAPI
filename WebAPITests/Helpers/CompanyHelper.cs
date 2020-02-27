using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Models;

namespace WebAPITests.Helpers
{
    public class CompanyHelper
    {
        protected readonly ApiContext _context;
        public CompanyHelper(ApiContext context)
        {
            _context = context;
        }

        public void AddCompanies()
        {
            _context.Companies.AddRange(
                    new Company
                    {
                        Id = 1,
                        Name = "Apple Inc.",
                        Exchange = "NASDAQ",
                        Ticker = "AAPL",
                        Isin = "US0378331005",
                        Website = "ApiContext"
                    },
                    new Company
                    {
                        Id = 2,
                        Name = "British Airways Plc",
                        Exchange = "Pink Sheets",
                        Ticker = "BAIRY",
                        Isin = "US1104193065",
                        Website = null
                    },
                    new Company
                    {
                        Id = 3,
                        Name = "Heineken NV",
                        Exchange = "Euronext Amsterdam",
                        Ticker = "HEIA",
                        Isin = "NL0000009165",
                        Website = null
                    },
                    new Company
                    {
                        Id = 4,
                        Name = "Panasonic Corp",
                        Exchange = "Tokyo Stock Exchange",
                        Ticker = "6752",
                        Isin = "JP3866800000",
                        Website = "http://www.panasonic.co.jp"
                    },
                    new Company
                    {
                        Id = 5,
                        Name = "Porsche Automobil",
                        Exchange = "Deutsche Börse",
                        Ticker = "PAH3",
                        Isin = "DE000PAH0038",
                        Website = "https://www.porsche.com/"
                    });

            _context.SaveChanges();
        }

        public Company CreateCompany(int id, string name, string exchange, string ticker,string isin, string website)
        {
            return new Company { Id = id, Name = name, Exchange = exchange, Ticker = ticker, Isin = isin, Website = website };
        }
    }
}
