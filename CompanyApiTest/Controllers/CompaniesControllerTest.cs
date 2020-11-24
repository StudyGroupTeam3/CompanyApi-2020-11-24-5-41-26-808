using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CompanyApi;
using CompanyApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace CompanyApiTest.Controllers
{
    public class CompaniesControllerTest
    {
        [Fact]
        public async Task Should_Create_A_New_Company_When_Post_A_New_Company()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("companies/clear");
            Company company = new Company(null, "Baymax");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            // when
            var response = await client.PostAsync("/companies", requestBody);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // then
            Assert.Equal("0", responseString);
        }

        [Fact]
        public async Task Should_Not_Create_Company_When_Post_An_Existed_Company()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("companies/clear");
            Company company = new Company(null, "Baymax");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/companies", requestBody);
            response.EnsureSuccessStatusCode();
            // when
            Company duplicatedCompany = new Company(null, "Baymax");
            string secondRequest = JsonConvert.SerializeObject(duplicatedCompany);
            StringContent secondRequestBody = new StringContent(secondRequest, Encoding.UTF8, "application/json");
            var secondResponse = await client.PostAsync("/companies", secondRequestBody);
            var responseString = await secondResponse.Content.ReadAsStringAsync();
            // then
            Assert.Equal(409, (int)secondResponse.StatusCode);
        }
    }
}
