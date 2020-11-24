using CompanyApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {
        private static List<Company> companies = new List<Company>();

        [HttpPost]
        public Company AddCompany(Company company)
        {
            company.CompanyId = companies.Count.ToString();
            companies.Add(company);
            return company;
        }

        [HttpGet]
        public List<Company> GetCompanies()
        {
            return companies;
        }

        [HttpGet("{companyId}")]
        public Company GetCompanyByCompanyId(string companyId)
        {
            return companies.FirstOrDefault(company => company.CompanyId == companyId);
        }

        [HttpPatch("{companyId}")]
        public void ModifyName(string companyId, Update updateData)
        {
            var companyFound = companies.FirstOrDefault(company => company.CompanyId == companyId);
            if (companyFound != null)
            {
                companyFound.Name = updateData.Name;
            }
        }

        [HttpPost("{companyID}/employees")]
        public Employee AddEmployee(string companyId, Employee employee)
        {
            var companyFound = companies.FirstOrDefault(company => company.CompanyId == companyId);
            return companyFound?.AddEmployee(employee);
        }

        [HttpDelete]
        public void Clear()
        {
            companies.Clear();
        }
    }
}
