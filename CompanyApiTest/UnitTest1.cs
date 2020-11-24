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
    public class UnitTest1
    {
        [Fact]
        public async Task Should_Add_Company_Successfully()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            NameGenerator name = new NameGenerator("Tecent");
            string request = JsonConvert.SerializeObject(name);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            var response1 = await client.GetAsync("CompanyApi/companies/{name}");
            var responseString1 = await response1.Content.ReadAsStringAsync();
            Company expectCompany = JsonConvert.DeserializeObject<Company>(responseString1);

            //when
            var response = await client.PostAsync("CompanyApi/companies", requestBody);

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
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            NameGenerator name = new NameGenerator("Tecent");
            NameGenerator name1 = new NameGenerator("Baidu");
            string request = JsonConvert.SerializeObject(name);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            string request1 = JsonConvert.SerializeObject(name1);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.DeleteAsync("CompanyApi/clear");
            await client.PostAsync("CompanyApi/companies", requestBody);
            await client.PostAsync("CompanyApi/companies", requestBody1);
            var expectCompanyNames = new List<string>() { "Tecent", "Baidu" };

            //when
            var response = await client.GetAsync("CompanyApi/companies");
            var responseString = await response.Content.ReadAsStringAsync();
            List<Company> actualCompanies = JsonConvert.DeserializeObject<List<Company>>(responseString);

            //then
            response.EnsureSuccessStatusCode();
            Assert.Equal(expectCompanyNames, actualCompanies.Select(com => com.CompanyName).ToList());
        }
    }
}
