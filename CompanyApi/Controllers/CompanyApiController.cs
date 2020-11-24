using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyApiController : ControllerBase
    {
        private static List<Company> companies = new List<Company>();

        [HttpPost("companies")]
        public Company AddNewCompany(NameGenerator generator)
        {
            var newCompany = new Company(generator.Name);
            if (companies.Any(com => com.CompanyName == generator.Name))
            {
                return null;
            }

            companies.Add(newCompany);
            return newCompany;
        }

        [HttpGet("companies/{name}")]
        public Company GetCompanyByName(string name)
        {
            return companies.FirstOrDefault(com => com.CompanyName == name);
        }

        [HttpGet("companies")]
        public List<Company> GetAllCompany()
        {
            return companies;
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            companies.Clear();
        }
    }
}
