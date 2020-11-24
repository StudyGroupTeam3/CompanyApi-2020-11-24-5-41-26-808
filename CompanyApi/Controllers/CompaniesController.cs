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
        public List<Company> GetCompanies(int pageSize, int pageIndex)
        {
            return pageSize == 0 && pageIndex == 0
                ? companies
                : companies.GetRange(pageSize * (pageIndex - 1), pageSize);
        }

        [HttpGet("{companyId}")]
        public Company GetCompanyByCompanyId(string companyId)
        {
            return companies.FirstOrDefault(company => company.CompanyId == companyId);
        }

        [HttpPatch("{companyId}")]
        public void ModifyInformationOfCompany(string companyId, Update updateData)
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

        [HttpGet("{companyID}/employees")]
        public List<Employee> GetEmployeeList(string companyId)
        {
            return companies.FirstOrDefault(company => company.CompanyId == companyId)?.GetEmployees();
        }

        [HttpPatch("{companyID}/employees/{employeeID}")]
        public void ModifyInformationOfEmployee(string companyId, string employeeId, Update updateData)
        {
            var employeeFound = companies.FirstOrDefault(company => company.CompanyId == companyId)
                ?.GetEmployees()
                .FirstOrDefault(employee => employee.EmployeeID == employeeId);
            if (employeeFound != null)
            {
                employeeFound.Name = updateData.Name;
            }
        }

        [HttpDelete("{companyID}/employees/{employeeID}")]
        public void DeleteEmployeeOfSpecificCompany(string companyId, string employeeId)
        {
            var companyFound = companies.FirstOrDefault(company => company.CompanyId == companyId);
            var employeeFound = companyFound?.GetEmployees().FirstOrDefault(employee => employee.EmployeeID == employeeId);
            companyFound?.GetEmployees().Remove(employeeFound);
        }

        [HttpDelete("{companyID}")]
        public void DeleteCompany(string companyId)
        {
            var companyFound = companies.FirstOrDefault(company => company.CompanyId == companyId);
            companies.Remove(companyFound);
        }

        [HttpDelete]
        public void Clear()
        {
            companies.Clear();
        }
    }
}
