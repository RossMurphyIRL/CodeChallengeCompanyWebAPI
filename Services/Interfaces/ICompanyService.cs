using Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICompanyService
    {
        bool IsinIsValid(string isin);
        bool IsinExists(CompanyDto company);
    }
}
