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
            List<Company> pageCompanies = companies.Where(company =>
            {
                if (companies.IndexOf(company) < pageSize * (pageIndex - 1))
                {
                    return false;
                }

                return companies.IndexOf(company) < pageSize * pageIndex;
            }).ToList();
            return pageCompanies;
        }

        [HttpPut("companies/{companyId}")]
        public Company UpdateCompanyInformation(string companyId, UpdateCompany updateCompany)
        {
            int index = companies.IndexOf(companies.Find(company => company.Id == companyId));
            companies[index].Name = updateCompany.Name;
            return companies[index];
        }

        [HttpPatch("companies/{companyId}/employees")]
        public List<Employee> AddEmployeeToCompany(string companyId, Employee employee)
        {
            int index = companies.IndexOf(companies.Find(company => company.Id == companyId));
            companies[index].Employees = new List<Employee> { employee };
            return companies[index].Employees;
        }

        [HttpGet("companies/{companyId}/employees")]
        public List<Employee> GetEmployeesInCompany(string companyId)
        {
            int index = companies.IndexOf(companies.Find(company => company.Id == companyId));
            return companies[index].Employees;
        }

        [HttpPatch("companies/{companyId}/{employeeId}")]
        public Employee UpdateEmployeeInformation(string companyId, string employeeId, UpdateEmployee updateEmployee)
        {
            int companyIndex = companies.IndexOf(companies.Find(company => company.Id == companyId));
            int employIndex = companies[companyIndex].Employees.IndexOf(companies[companyIndex].Employees.Find(employ => employ.Id == employeeId));
            companies[companyIndex].Employees[employIndex].Name = updateEmployee.Name;
            companies[companyIndex].Employees[employIndex].Salary = updateEmployee.Salary;
            return companies[companyIndex].Employees[employIndex];
        }

        [HttpDelete("companies/{companyId}/{employeeId}")]
        public void DeleteEmployeeInformation(string companyId, string employeeId)
        {
            int companyIndex = companies.IndexOf(companies.Find(company => company.Id == companyId));
            int employIndex = companies[companyIndex].Employees.IndexOf(companies[companyIndex].Employees.Find(employ => employ.Id == employeeId));
            companies[companyIndex].Employees.RemoveAt(employIndex);
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
