using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {
        private static List<Company> companies = new List<Company>();

        [HttpPost]
        public ActionResult<Company> AddCompany(Company company)
        {
            List<string> companiesNames = companies.Select(x => x.Name).ToList();
            if (companiesNames.Contains(company.Name))
            {
                return Conflict(company);
            }

            companies.Add(company);
            return Ok(company);
        }

        [HttpGet]
        public ActionResult<List<Company>> GetAllCompanies()
        {
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public ActionResult<Company> GetOneCompany(string id)
        {
            var company = companies.FirstOrDefault(x => x.CompanyId == id);
            if (company == null)
            {
               return NotFound(company);
            }

            return Ok(company);
        }

        [HttpDelete]
        public void Clear()
        {
            companies.Clear();
        }
    }
}
