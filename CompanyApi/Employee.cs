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

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public double Salary { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Employee))
            {
                return false;
            }

            Employee employee = (Employee)obj;
            return employee.Name == Name && employee.Salary == Salary && employee.Id == Id;
        }
    }
}
