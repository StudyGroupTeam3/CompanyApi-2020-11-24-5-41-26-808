using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CompanyApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace CompanyApiTest
{
    public class CompanyApiTes
    {
        private readonly TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        private readonly HttpClient client;
        public CompanyApiTes()
        {
            client = server.CreateClient();
            client.DeleteAsync("CompanyApi/clear");
        }

        [Fact]
        public async Task Should_Add_Company_Successfully()
        {
            //given
            NameGenerator name = new NameGenerator("Tecent");
            string request = JsonConvert.SerializeObject(name);
            string testName = "Tecent";
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            //when
            var response = await client.PostAsync("CompanyApi/companies", requestBody);
            var response1 = await client.GetAsync($"CompanyApi/companies/{testName}");
            var responseString1 = await response1.Content.ReadAsStringAsync();
            Company expectCompany = JsonConvert.DeserializeObject<Company>(responseString1);

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actualCompany = JsonConvert.DeserializeObject<Company>(responseString);
            Assert.Equal(expectCompany, actualCompany);
        }

        [Fact]
        public async Task Should_Get_All_Company_Return_Correct_Companies()
        {
            //given
            var expectCompanyNames = await GenerateCompanies();

            //when
            var response = await client.GetAsync("CompanyApi/companies");
            var responseString = await response.Content.ReadAsStringAsync();
            List<Company> actualCompanies = JsonConvert.DeserializeObject<List<Company>>(responseString);

            //then
            response.EnsureSuccessStatusCode();
            Assert.Equal(expectCompanyNames.Select(item => item.Name), actualCompanies.Select(com => com.CompanyName));
        }

        [Fact]
        public async Task Should_Get_CompanybyName_Return_Correct_Company()
        {
            //given
            var expectCompanyNames = await GenerateCompanies();
            var expectedName = "Tecent";

            //when
            var response = await client.GetAsync($"CompanyApi/companies/{expectedName}");
            var responseString = await response.Content.ReadAsStringAsync();
            Company actualCompanies = JsonConvert.DeserializeObject<Company>(responseString);

            //then
            response.EnsureSuccessStatusCode();
            Assert.Equal(expectedName, actualCompanies.CompanyName);
        }

        private async Task<List<NameGenerator>> GenerateCompanies()
        {
            var companyNames = new List<NameGenerator>()
            {
                new NameGenerator("Tecent"),
                new NameGenerator("Baidu"),
                new NameGenerator("Ali"),
            };

            foreach (var requestBody in companyNames.Select(JsonConvert.SerializeObject)
                .Select(request => new StringContent(request, Encoding.UTF8, "application/json")))
            {
                await client.PostAsync("CompanyApi/companies", requestBody);
            }

            return companyNames;
        }
    }
}
