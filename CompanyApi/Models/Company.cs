using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Company
    {
        public Company()
        {
        }

        public Company(string name)
        {
            Name = name;
            CompanyId = "1234";
        }

        public string CompanyId { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            var company = (Company)obj;
            return company != null && company.Name == Name && company.CompanyId == CompanyId;
        }
    }
}
