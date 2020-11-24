using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Employee
    {
        public Employee(string employeeID, string name, int salary)
        {
            EmployeeID = employeeID;
            Name = name;
            Salary = salary;
        }

        public string EmployeeID { get; }
        public string Name { get; }
        public int Salary { get; }
    }
}
