﻿using System;
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
            if (companies.Any(com => com.CompanyName == generator.Name))
            {
                var newCompany = new Company(generator.Name);
                companies.Add(newCompany);
                return newCompany;
            }

            return null;
        }

        [HttpGet("companies")]
        public Company GetCompanyByName(string name)
        {
            return companies.FirstOrDefault(com => com.CompanyName == name);
        }
    }
}
