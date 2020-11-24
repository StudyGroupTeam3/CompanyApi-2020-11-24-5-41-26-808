using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CompanyApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace CompanyApiTest.Controllers
{
    public class CompaniesControllerTest
    {
        private readonly TestServer server;
        private readonly HttpClient client;

        public CompaniesControllerTest()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();
            client.DeleteAsync("Companies/");
        }

        [Fact]
        public async Task Should_Add_Company_Give_The_Name()
        {
            Company company = new Company(name: "YiBao");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var response = await client.PostAsync("Companies/", requestBody);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company acutalCompany = JsonConvert.DeserializeObject<Company>(responseString);
            Assert.Equal(company.Name, acutalCompany.Name);
            Assert.NotNull(acutalCompany.CompanyId);
        }

        [Fact]
        public async Task Should_Return_Conflict_When_Add_Company_With_The_Same_Name()
        {
            Company company1 = new Company(name: "WaHaHa");
            Company company2 = new Company(name: "WaHaHa");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("Companies/", requestBody1);

            // when
            var response = await client.PostAsync("Companies/", requestBody2);

            // then
            Assert.True(response.StatusCode == HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Should_Return_All_Companies_When_Get_All_Companies()
        {
            // when
            Company company1 = new Company(name: "LongFuShanQuan");
            Company company2 = new Company(name: "JianLiBao");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("Companies/", requestBody1);
            await client.PostAsync("Companies/", requestBody2);

            var response = await client.GetAsync("Companies/");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var acutalCompanyList = JsonConvert.DeserializeObject<List<Company>>(responseString);
            Assert.Equal(new List<string>() { company1.Name, company2.Name }, acutalCompanyList.Select(x => x.Name).ToList());
        }

        [Fact]
        public async Task Should_Return_An_Existing_Company()
        {
            // when
            Company company = new Company(name: "DiDi");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("Companies/", requestBody);

            var response = await client.GetAsync($"Companies/{company.CompanyId}");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var acutalCompany = JsonConvert.DeserializeObject<Company>(responseString);
            Assert.Equal(company.Name, acutalCompany.Name);
        }
    }
}
