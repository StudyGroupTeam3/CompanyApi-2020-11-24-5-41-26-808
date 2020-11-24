using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi
{
    public class UpdateCompany
    {
        public UpdateCompany()
        {
        }

        public UpdateCompany(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
