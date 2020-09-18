using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly string login = "admin";
        private readonly string password = "admin123";

        public CompanyController(DatabaseContext context)
        {
            _context = context;
        }

        private static string GetEncoded(string value) => System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(value));

        // PUT: company/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCompanyById(long id, Company company)
        {
            var headerValues = Request.Headers.Values;
            var encodedLogin = GetEncoded(headerValues.ElementAt(7).ToString());
            var encodedPassword = GetEncoded(headerValues.ElementAt(8).ToString());

            if ((encodedLogin == login) && (encodedPassword == password))
            {
                if (!CompanyExists(id))
                {
                    return BadRequest();
                }

                company.CompanyId = id;

                if ((company.Name == null) || (company.EstablishmentYear == null))
                {
                    return BadRequest("You must provide Company Name and Establishment Year. List of employees is optional");
                }

                _context.Entry(company).State = EntityState.Modified; // TODO: fix a bug: employees does not change.

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
                return Unauthorized();
            }
        }

        // POST: company/create
        [HttpPost("create")]
        public async Task<ActionResult<CompanyDTO>> CreateCompany(Company company)
        {
            var headerValues = Request.Headers.Values;
            var encodedLogin = GetEncoded(headerValues.ElementAt(7).ToString());
            var encodedPassword = GetEncoded(headerValues.ElementAt(8).ToString());

            if ((encodedLogin == login) && (encodedPassword == password))
            {
                if ((company.Name == null) || (company.EstablishmentYear == null))
                {
                    return BadRequest("Name and Establishment Year are required");
                }

                _context.Companies.Add(company);
                await _context.SaveChangesAsync();

                var companyDTO = new CompanyDTO
                {
                    CompanyId = company.CompanyId
                };

                return Ok(companyDTO);
            }
            else
            {
                return Unauthorized();
            }
        }

        // POST: company/search
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<Company>>> SearchCompanyByParameters(CompanySearchingParameters parameters)
        {

            return await _context.Companies
            .Include(x => x.Employees)
            .Where(x => x.Name.Contains(parameters.Keyword) ||
                        x.Employees.Any(e => e.FirstName.Contains(parameters.Keyword)) ||
                        x.Employees.Any(e => e.LastName.Contains(parameters.Keyword)) ||
                        x.Employees.Any(e => (e.DateOfBirth >= parameters.EmployeeDateOfBirthFrom) && (e.DateOfBirth <= parameters.EmployeeDateOfBirthTo)) ||
                        x.Employees.Any(e => e.DateOfBirth >= parameters.EmployeeDateOfBirthFrom) ||
                        x.Employees.Any(e => e.DateOfBirth <= parameters.EmployeeDateOfBirthTo) ||
                        x.Employees.Any(e => e.JobTitle.Equals(parameters.EmployeeJobTitles)))
                        // TODO: more rules...
            .ToListAsync();
        }

        // DELETE: company/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Company>> DeleteCompanyById(long id)
        {

            var headerValues = Request.Headers.Values;
            var encodedLogin = GetEncoded(headerValues.ElementAt(5).ToString());
            var encodedPassword = GetEncoded(headerValues.ElementAt(6).ToString());

            if ((encodedLogin == login) && (encodedPassword == password))
            {
                var company = await _context.Companies.FindAsync(id);

                if (company == null)
                {
                    return NotFound();
                }

                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool CompanyExists(long id) => _context.Companies.Any(e => e.CompanyId == id);
    }
}
