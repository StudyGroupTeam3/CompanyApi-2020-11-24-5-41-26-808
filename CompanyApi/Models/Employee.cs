using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Employee
    {
        public Employee()
        {
        }

        public Employee(string employeeID, string name, int salary)
        {
            EmployeeID = employeeID;
            Name = name;
            Salary = salary;
        }

        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }

        public override bool Equals(object? obj)
        {
            var employee = (Employee)obj;
            return employee != null && employee.Name == Name && employee.EmployeeID == EmployeeID;
        }
    }
}
