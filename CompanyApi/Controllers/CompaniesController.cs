using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyApi.Models;
using Microsoft.AspNetCore.Mvc;

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
            companies.Add(company);
            return company;
        }

        [HttpDelete]
        public void Clear()
        {
            companies.Clear();
        }
    }
}
