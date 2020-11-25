using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using CompanyApi.Models;
using Microsoft.AspNetCore.CookiePolicy;
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
        public async Task<ActionResult<Company>> AddCompany(Company company)
        {
            if (companies.Count != 0 && companies.Where(idCompanyPair => idCompanyPair.Value.Name == company.Name).ToList().Count > 0)
            {
                return Conflict();
            }

            string companyID = GenerateCompanyID();
            Company newCompany = new Company(companyID, company.Name);
            newCompany.Employees = company.Employees;
            companies[companyID] = newCompany;
            return CreatedAtRoute(nameof(GetCompanyByID), new { id = newCompany.CompanyID }, newCompany);
        }

        [HttpGet]
        public List<Company> GetAllCompany(int? pageSize, int? startPage)
        {
            if (pageSize.HasValue && startPage.HasValue)
            {
                return companies.Select(idCompanyPair => idCompanyPair.Value).Skip((startPage.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
            }

            return companies.Select(idCompanyPair => idCompanyPair.Value).ToList();
        }

        [HttpGet("{id}", Name = "GetCompanyByID")]
        public Company GetCompanyByID(string id)
        {
            return companies[id];
        }

        //[HttpGet("page")]
        //public List<Company> GetCompanyByPage(int pageSize, int startPage)
        //{
        //    return companies.Select(idCompanyPair => idCompanyPair.Value).Skip((startPage - 1) * pageSize).Take(pageSize).ToList();
        //}

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateCompany(string id, CompanyUpdateModel companyUpdateModel)
        {
            if (companies.Count != 0 && companies.Where(idCompanyPair => idCompanyPair.Value.Name == companyUpdateModel.Name).ToList().Count > 0)
            {
                return Conflict();
            }

            companies.FirstOrDefault(idCompanyPair => idCompanyPair.Value.CompanyID == id).Value.Name =
                companyUpdateModel.Name;
            return Ok();
        }

        [HttpPost("{id}/employees")]
        public async Task<ActionResult<Company>> AddEmployee(string id, Employee employee)
        {
            var company = companies[id];

            var newEmployee = new Employee(company.GenerateCEmployeeID(), employee.Name, employee.Salary);
            company.Employees[newEmployee.EmployeeId] = newEmployee;
            return Ok(newEmployee);
        }

        [HttpGet("{companyID}/employees")]
        public List<Employee> GetAllEmployee(string companyID)
        {
            return companies[companyID].Employees.Values.ToList();
        }

        [HttpPatch("{companyID}/employees/{employeeID}")]
        public async Task<ActionResult> UpdateEmployee(string companyID, string employeeID, EmployeeUpdateModel employeeUpdateModel)
        {
            var employee = companies[companyID].Employees[employeeID];
            employee.Name = employeeUpdateModel.Name is null ? employee.Name : employeeUpdateModel.Name;
            employee.Salary = employeeUpdateModel.Salary.HasValue
                ? employeeUpdateModel.Salary.Value
                : employee.Salary;
            return Ok();
        }

        [HttpDelete("{companyID}/employees/{employeeID}")]
        public async Task<ActionResult> DeleteEmployee(string companyID, string employeeID)
        {
            var company = companies[companyID];
            company.Employees.Remove(employeeID);
            return Ok();
        }

        [HttpDelete("{companyID}")]
        public async Task<ActionResult> DeleteCompany(string companyID)
        {
            companies.Remove(companyID);
            return Ok();
        }

        private string GenerateCompanyID()
        {
            string companyIDGenerated = new string(companyID);
            companyID = (uint.Parse(companyID) + 1).ToString();
            return companyIDGenerated;
        }
    }
}
