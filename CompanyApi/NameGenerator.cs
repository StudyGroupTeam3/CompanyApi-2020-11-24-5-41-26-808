using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi
{
    public class NameGenerator
    {
        public NameGenerator(string name)
        {
            Name = name;
        }

        public NameGenerator()
        {
        }

        public string Name { get; set; }
    }
}
