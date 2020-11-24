using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace CompanyApi
{
    public class Company
    {
        private static List<int> idNumbers = new List<int>();

        public Company()
        {
        }

        public Company(string name)
        {
            CompanyName = name;
            CompanyID = GenerateID();
        }

        public string CompanyID { get; }

        public string CompanyName { get; set; }

        private string GenerateID()
        {
            Random rnd = new Random();
            var minNum = 10000;
            var maxNum = 99999;
            int idNumber = rnd.Next(minNum, maxNum);
            while (idNumbers.Contains(idNumber))
            {
                idNumber = rnd.Next(minNum, maxNum);
            }

            idNumbers.Add(idNumber);
            return $"CompanyID_{idNumber}";
        }
    }
}
