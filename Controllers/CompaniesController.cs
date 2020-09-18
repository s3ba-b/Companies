using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestBackendDeveloper.Models;

namespace TestBackendDeveloper.Controllers
{
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
            var headerValues = Request.Headers.Values;
            var encodedLogin = GetEncoded(headerValues.ElementAt(5).ToString());
            var encodedPassword = GetEncoded(headerValues.ElementAt(6).ToString());

            if (encodedLogin == "admin" && encodedPassword == "admin123")
            {
                return await _context.Companies
                .Include(x => x.Employees)
                .ToListAsync();
            }
            else
            {
                return BadRequest("Wrong credentials!");
            }
        }

        private static string GetEncoded(string value) => System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(value));

        // GET: company/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(long id)
        {

            var headerValues = Request.Headers.Values;
            var encodedLogin = GetEncoded(headerValues.ElementAt(5).ToString());
            var encodedPassword = GetEncoded(headerValues.ElementAt(6).ToString());

            if (encodedLogin == "admin" && encodedPassword == "admin123")
            {
                var company = await _context.Companies
            .Where(x => x.CompanyId.Equals(id))
            .Include(x => x.Employees)
            .SingleOrDefaultAsync();

                if (company == null)
                {
                    return NotFound();
                }

                return company;
            }
            else
            {
                return BadRequest("Wrong credentials!");
            }
        }

        // PUT: company/update/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCompany(long id, Company company)
        {

            var headerValues = Request.Headers.Values;
            var encodedLogin = GetEncoded(headerValues.ElementAt(5).ToString());
            var encodedPassword = GetEncoded(headerValues.ElementAt(6).ToString());

            if (encodedLogin == "admin" && encodedPassword == "admin123")
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
            else
            {
                return BadRequest("Wrong credentials!");
            }
        }

        // POST: company/create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("create")]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            var headerValues = Request.Headers.Values;
            var encodedLogin = GetEncoded(headerValues.ElementAt(5).ToString());
            var encodedPassword = GetEncoded(headerValues.ElementAt(6).ToString());
            
            if (encodedLogin == "admin" && encodedPassword == "admin123")
            {
                if (company.Name == null)
                {
                    return BadRequest();
                }

                _context.Companies.Add(company);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCompany), new { id = company.CompanyId }, company);
            }
            else
            {
                return BadRequest("Wrong credentials!");
            }
        }

        // POST: company/search
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<Company>>> SearchCompany(CompanySearchingParameters parameters)
        {

            if (parameters.Keyword == null)
            {
                parameters.Keyword = "";
            }

            return await _context.Companies
            .Include(x => x.Employees)
                .Where(x =>
                    x.Name.Equals(parameters.Keyword) ||
                    x.Employees.Any(e => e.FirstName.Contains(parameters.Keyword)) ||
                    x.Employees.Any(e => e.LastName.Contains(parameters.Keyword)) ||
                    x.Employees.Any(e => e.DateOfBirth >= parameters.EmployeeDateOfBirthFrom && e.DateOfBirth <= parameters.EmployeeDateOfBirthTo) ||
                    // TODO: Not working - fix that
                    x.Employees.Any(e => e.JobTitle.Equals(parameters.EmployeeJobTitles)))
                        .ToListAsync();
        }

        // DELETE: company/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(long id)
        {

            var headerValues = Request.Headers.Values;
            var encodedLogin = GetEncoded(headerValues.ElementAt(5).ToString());
            var encodedPassword = GetEncoded(headerValues.ElementAt(6).ToString());

            if (encodedLogin == "admin" && encodedPassword == "admin123")
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
            else
            {
                return BadRequest("Wrong credentials!");
            }
        }

        private bool CompanyExists(long id) => _context.Companies.Any(e => e.CompanyId == id);
    }
}
