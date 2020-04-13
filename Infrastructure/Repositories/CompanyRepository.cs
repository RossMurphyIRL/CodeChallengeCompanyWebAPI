using Core;
using Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure
{
    public class CompanyRepository : ICompanyContext
    {
        private readonly CompanyContext _context;
        public CompanyRepository(CompanyContext context)
        {
            _context = context;
        }
        public async Task<CompanyDto> Add(CompanyDto company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public void Delete(CompanyDto company)
        {
            _context.Companies.Remove(company);
            _context.SaveChangesAsync();
        }

        public Task<List<CompanyDto>> GetCompanies()
        {
            return _context.Companies.ToListAsync();
        }

        public Task<CompanyDto> GetCompany(int id)
        {
            return _context.Companies.SingleOrDefaultAsync(x=>x.Id == id);
        }

        public Task<CompanyDto> GetCompanyByIsin(string isin)
        {
            return _context.Companies.SingleOrDefaultAsync(x => x.Isin.ToLower().Contains(isin.ToLower()));
        }
        

        public Task<int> UpdateCompany(CompanyDto company)
        {
            _context.Entry(company).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<CompanyDto> GetCompanyByIsinWithoutCompany(CompanyDto company)
        {
            return _context.Companies.SingleOrDefaultAsync(x => x.Isin.ToLower().Contains(company.Isin.ToLower()) && x.Id != company.Id);
        }

        public bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
