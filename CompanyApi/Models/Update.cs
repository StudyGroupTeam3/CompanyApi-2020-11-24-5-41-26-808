using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Update
    {
        public Update()
        {
        }

        public Update(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
