﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CompanyApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private static List<Company> companies = new List<Company>();
        [HttpPost("companies")]
        public Company AddCompany(Company company)
        {
            if (companies.Count(thisCompany => thisCompany.Name == company.Name) == 0)
            {
                companies.Add(company);
            }

            return company;
        }

        [HttpGet("companies")]
        public List<Company> GetAllCompanies()
        {
            return companies;
        }

        [HttpGet("companies/{companyId}")]
        public Company GetCompanyById(string companyId)
        {
            return companies.Find(thisCompany => thisCompany.Id == companyId);
        }

        [HttpGet("companies/pages/{pageIndex}")]
        public List<Company> GetCompaniesInOnePage(int pageSize, int pageIndex)
        {
            //  I can obtain X(page size) companies from index of Y(page index start from 1)
            return null;
        }

        [HttpPut("companies/{companyId}")]
        public Company UpdateCompanyInformation(string companyId)
        {
            //  I can update basic information of an existing company
            return null;
        }

        [HttpPost("companies/{companyId}/employees")]
        public Company AddEmployeeToCompany(string companyId)
        {
            //  I can add an employee to a specific company
            return null;
        }

        [HttpGet("companies/{companyId}/employees")]
        public List<Employee> GetEmployeesInCompany(string companyId)
        {
            //  I can obtain list of all employee under a specific company
            return null;
        }

        [HttpPut("companies/{companyId}/{employeeId}")]
        public List<Employee> UpdateEmployeeInformation(string companyId, string employeeId)
        {
            //  I can update basic information of a specific employee under a specific company
            return null;
        }

        [HttpDelete("companies/{companyId}/{employeeId}")]
        public void DeleteEmployeeInformation(string companyId, string employeeId)
        {
            //  I can delete a specific employee under a specific company.
            return;
        }

        [HttpDelete("companies/{companyId}")]
        public void DeleteCompanyAndEmployeesInformation(string companyId)
        {
            //  I can delete a specific company, and all employees belong to this company should also be deleted
            return;
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            companies.Clear();
        }
    }
}
