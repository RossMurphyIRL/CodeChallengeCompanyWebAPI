using Core;
using Core.Interfaces;
using Services.Interfaces;
using Services.Validators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Services.CustomExceptions.CompanyExceptions;

namespace Services.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyContext _companyRepository;
        public CompanyService(ICompanyContext companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public bool IsinExists(CompanyDto company)
        {
            if (company == null || company.Isin == null) return false;
            var existingCompanyForIsin = _companyRepository.GetCompanyByIsinWithoutCompany(company);
            if (existingCompanyForIsin.Result != null)
            {
                return true;
            }
            return false;
        }

        public bool IsinIsValid(string isin)
        {
            if (isin == null) return false;
            var companyIsinValidator = new CompanyIsinValidator();
            if (!companyIsinValidator.Validate(isin).IsValid)
            {
                return false;
            }
            return true;
        }
    }
}
