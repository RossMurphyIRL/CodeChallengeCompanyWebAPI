using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyContext _companyRepository;
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyContext companyRepository, ICompanyService companyService)
        {
            _companyRepository = companyRepository;
            _companyService = companyService;
        }

        // GET: api/Companies
        /// <summary>
		/// Gets list of all companies
		/// </summary>
        [HttpGet]
        public async Task<ActionResult<List<CompanyDto>>> GetCompanies()
        {
            return await _companyRepository.GetCompanies();
        }

        // GET: api/Companies/5
        /// <summary>
		/// Gets company for specified id
		/// </summary>
		/// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            var company = await _companyRepository.GetCompany(id);

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
        public async Task<ActionResult<CompanyDto>> GetCompanyByIsin(string isin)
        {
            if (!_companyService.IsinIsValid(isin))
            {
                return BadRequest("Requested isin must be in correct format.");
            }
            var company = await _companyRepository.GetCompanyByIsin(isin);

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
        public async Task<IActionResult> PutCompany(int id, CompanyDto company)
        {
            if (id != company.Id || id ==0)
            {
                return BadRequest("Company Id not valid");
            }
            if (_companyService.IsinExists(company))
            {
                return BadRequest("Company with isin: " + company.Isin + " already exists.");
            }
            var updateResult = await _companyRepository.UpdateCompany(company);
            if (updateResult > 0)
            {
                return NoContent();
            } else
            {
                return BadRequest("Error updating company");
            }
        }

        // POST: api/Companies
        /// <summary>
		/// Add new company
		/// </summary>
		/// <param name="company"></param>
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> PostCompany(CompanyDto company)
        {
            if (_companyService.IsinExists(company))
            {
                return BadRequest("Company with isin: " + company.Isin + " already exists.");
            }
            company = await _companyRepository.Add(company);
            
            return CreatedAtAction("GetCompany", new { id = company.Id }, company);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CompanyDto>> DeleteCompany(int id)
        {
            var company = await _companyRepository.GetCompany(id);
            if (company == null)
            {
                return NotFound("Can not find company");
            }
            _companyRepository.Delete(company);

            return company;
        }

    }
}
