using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {
        private static SortedDictionary<string, Company> companies = new SortedDictionary<string, Company>();
        private string companyID = "0";

        [HttpDelete("clear")]
        public async Task ClearCompany()
        {
            companies.Clear();
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddCompany(Company company)
        {
            if (companies.Count != 0 && companies.Where(idCompanyPair => idCompanyPair.Value.Name == company.Name).ToList().Count > 0)
            {
                return Conflict();
            }

            string companyID = GenerateCompanyID();
            companies[companyID] = new Company(companyID, company.Name);
            return Created(string.Empty, companyID);
        }

        [HttpGet]
        public List<Company> GetAllCompany()
        {
            return companies.Select(idCompanyPair => idCompanyPair.Value).ToList();
        }

        private string GenerateCompanyID()
        {
            string companyIDGenerated = new string(this.companyID);
            this.companyID = (uint.Parse(this.companyID) + 1).ToString();
            return companyIDGenerated;
        }
    }
}
