using CompanyApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompanyApiTest
{
    public class CompanyControllerTest
    {
        private readonly TestServer server;
        private readonly HttpClient client;
        public CompanyControllerTest()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();
        }

        [Fact]
        public async Task Should_Add_Correct_Company_When_Add_Company()
        {
            // I can add a company if its name no same to any existing company
            // given
            Company company = new Company(name: "Apple");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var postResponse = await client.PostAsync("Company/companies", requestBody);
            var postResponseString = await postResponse.Content.ReadAsStringAsync();
            Company postCompany = JsonConvert.DeserializeObject<Company>(postResponseString);
            var response = await client.GetAsync($"Company/companies/{postCompany.Id}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actualCompany = JsonConvert.DeserializeObject<Company>(responseString);

            // then
            Assert.Equal(company, actualCompany);
        }
    }
}
