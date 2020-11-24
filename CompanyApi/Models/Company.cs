using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Company
    {
        private string employeeID = "0";

        public Company()
        {
        }

        public Company(string companyID, string name)
        {
            this.CompanyID = companyID;
            this.Name = name;
        }

        public string CompanyID { get; set; }
        public string Name { get; set; }
        public SortedDictionary<string, Employee> Employees { get; set; } = new SortedDictionary<string, Employee>();

        public string GenerateCEmployeeID()
        {
            string employeeIDGenerated = new string(employeeID);
            employeeID = (uint.Parse(employeeID) + 1).ToString();
            return employeeIDGenerated;
        }

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

            return Equals((Company)obj);
        }

        private bool Equals(Company other)
        {
            return CompanyID == other.CompanyID && Name == other.Name;
        }
    }

    public class CompanyUpdateModel
    {
        public CompanyUpdateModel()
        {
        }

        public CompanyUpdateModel(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
