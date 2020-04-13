using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICompanyContext
    {
        Task<CompanyDto> Add(CompanyDto company);
        Task<int> UpdateCompany(CompanyDto company);
        void Delete(CompanyDto company);
        Task<CompanyDto> GetCompany(int companyId);
        Task<List<CompanyDto>> GetCompanies();
        Task<CompanyDto> GetCompanyByIsin(string isin);
        Task<CompanyDto> GetCompanyByIsinWithoutCompany(CompanyDto company);

    }
}
