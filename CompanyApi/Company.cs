using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi
{
    public class Company
    {
        public Company()
        {
        }

        public Company(string name)
        {
            Name = name;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Company))
            {
                return false;
            }

            Company company = (Company)obj;
            return company.Name == Name && company.Id == Id;
        }
    }
}
