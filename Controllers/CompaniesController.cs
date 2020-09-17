using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestBackendDeveloper.Models;

namespace TestBackendDeveloper.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CompanyController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: company/list
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies
            .Include(x => x.Employees)
            .ToListAsync();
        }

        // GET: company/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(long id)
        {
            var company = await _context.Companies
            .Where(x => x.CompanyId == id)
            .Include(x => x.Employees)
            .SingleOrDefaultAsync();

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: company/update/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCompany(long id, Company company)
        {
            if (id != company.CompanyId)
            {
                return BadRequest();
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

        // POST: company/create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("create")]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            if(company.Name == null) {
                return BadRequest();
            }
            
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompany), new { id = company.CompanyId }, company);
        }

        // POST: company/search
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<Company>>> SearchCompany(CompanySearchingParameters parameters) {

            if(parameters.Keyword == null) {
                parameters.Keyword = ""; 
            }
            
            return await _context.Companies
            .Include(x => x.Employees)
            .Where(x =>
                x.Name.Equals(parameters.Keyword) ||
                x.Employees.Any(e => e.FirstName.Contains(parameters.Keyword)) ||
                x.Employees.Any(e => e.LastName.Contains(parameters.Keyword)) ||
                x.Employees.Any(e => e.DateOfBirth >= parameters.EmployeeDateOfBirthFrom && e.DateOfBirth <= parameters.EmployeeDateOfBirthTo) ||
                x.Employees.Any(e => e.JobTitle.Equals(parameters.EmployeeJobTitles)))
            .ToListAsync();
        }

        // DELETE: company/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(long id)
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

        private bool CompanyExists(long id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}
