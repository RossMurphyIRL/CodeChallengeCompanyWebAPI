using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Validators;
using static WebAPI.CustomExceptions.CustomExceptions;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ApiContext _context;

        public CompaniesController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        /// <summary>
		/// Gets list of all companies
		/// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        // GET: api/Companies/5
        /// <summary>
		/// Gets company for specified id
		/// </summary>
		/// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // GET: api/Companies/GetCompanyByIsin?isin=US1104193065
        /// <summary>
		/// Gets company for the specified isin
		/// </summary>
		/// <param name="id"></param>
        [HttpGet]
        [Route("GetCompanyByIsin")]
        [ActionName("GetCompanyByIsin")]
        public async Task<ActionResult<Company>> GetCompanyByIsin(string isin)
        {
            var companyIsinValidator = new CompanyIsinValidator();
            if (!companyIsinValidator.Validate(isin).IsValid)
            {
                throw new IsinException("Requested isin must be in correct format.");
            }
            var company = await _context.Companies.SingleOrDefaultAsync(x => x.Isin.ToLower().Contains(isin.ToLower()));

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/Companies/5
        /// <summary>
		/// Updates company for specified id
		/// </summary>
		/// <param name="id"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                throw new ModelStateException("Company model is in invalid state.");
            }

            var companyIsinValidator = new CompanyIsinValidator();
            if (!companyIsinValidator.Validate(company.Isin).IsValid)
            {
                throw new IsinException("Update Isin value must be in correct format.");
            }

            var existingCompanyForIsin = _context.Companies.SingleOrDefaultAsync(x => x.Isin.ToLower().Contains(company.Isin.ToLower()) && x.Id != company.Id);
            if (existingCompanyForIsin.Result != null)
            {
                throw new IsinException("Company with isin:" + company.Isin + " already exists.");
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Companies
        /// <summary>
		/// Add new company
		/// </summary>
		/// <param name="company"></param>
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException("Company model is in invalid state.");
            }
            var companyIsinValidator = new CompanyIsinValidator();
            if (!companyIsinValidator.Validate(company.Isin).IsValid)
            {
                throw new IsinException("Add Isin value must be in correct format.");
            }
            var existingCompanyForIsin = _context.Companies.SingleOrDefaultAsync(x => x.Isin.ToLower().Contains(company.Isin.ToLower()));
            if (existingCompanyForIsin.Result != null)
            {
                throw new IsinException("Company with isin:" + company.Isin + " already exists.");
            }
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.Id }, company);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return company;
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
