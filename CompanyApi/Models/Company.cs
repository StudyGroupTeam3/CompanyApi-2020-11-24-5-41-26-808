using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Company
    {
        private static List<Employee> employees = new List<Employee>();
        public Company()
        {
        }

        public Company(string name, string id)
        {
            Name = name;
            CompanyId = id;
        }

        public string CompanyId { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            var company = (Company)obj;
            return company != null && company.Name == Name && company.CompanyId == CompanyId;
        }

        public Employee AddEmployee(Employee employee)
        {
            employee.EmployeeID = employees.Count.ToString();
            employees.Add(employee);
            return employee;
        }

        public List<Employee> GetEmployees()
        {
            return employees;
        }
    }
}
