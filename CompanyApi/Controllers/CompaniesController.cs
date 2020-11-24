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
        private static string companyID = "0";

        [HttpDelete("clear")]
        public async Task ClearCompany()
        {
            companyID = "0";
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

        [HttpGet("{id}")]
        public Company GetCompanyByID(string id)
        {
            return companies[id];
        }

        [HttpGet("page")]
        public List<Company> GetCompanyByPage(int pageSize, int startPage)
        {
            return companies.Select(idCompanyPair => idCompanyPair.Value).Skip((startPage - 1) * pageSize).Take(pageSize).ToList();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<string>> UpdateCompany(string id, CompanyUpdateModel companyUpdateModel)
        {
            if (companies.Count != 0 && companies.Where(idCompanyPair => idCompanyPair.Value.Name == companyUpdateModel.Name).ToList().Count > 0)
            {
                return Conflict();
            }

            companies.FirstOrDefault(idCompanyPair => idCompanyPair.Value.CompanyID == id).Value.Name =
                companyUpdateModel.Name;
            return Created(string.Empty, id);
        }

        private string GenerateCompanyID()
        {
            string companyIDGenerated = new string(companyID);
            companyID = (uint.Parse(companyID) + 1).ToString();
            return companyIDGenerated;
        }
    }
}
