using System;
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
    }
}
