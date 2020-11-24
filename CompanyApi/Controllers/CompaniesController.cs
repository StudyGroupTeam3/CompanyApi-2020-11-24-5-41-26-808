using CompanyApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        [HttpDelete]
        public void Clear()
        {
            companies.Clear();
        }
    }
}
