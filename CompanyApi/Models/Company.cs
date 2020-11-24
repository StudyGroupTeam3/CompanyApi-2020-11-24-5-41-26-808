using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Company
    {
        public Company(string companyID, string name)
        {
            CompanyID = companyID;
            Name = name;
        }

        public string CompanyID { get; }
        public string Name { get; }
    }
}
