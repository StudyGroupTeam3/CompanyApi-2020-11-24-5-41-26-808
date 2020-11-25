using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi
{
    public class Employee
    {
        public Employee()
        {
        }

        public Employee(string name, double salary)
        {
            Name = name;
            Salary = salary;
        }

        public string Name { get; set; }
        public double Salary { get; set; }
    }
}
