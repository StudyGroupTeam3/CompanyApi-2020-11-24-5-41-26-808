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
            this.EmployeeId = employeeID;
            this.Name = name;
            this.Salary = salary;
        }

        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((Employee)obj);
        }

        private bool Equals(Employee other)
        {
            return EmployeeId == other.EmployeeId && Name == other.Name && Salary == other.Salary;
        }
    }
}
