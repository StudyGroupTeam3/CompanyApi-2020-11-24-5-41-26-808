using CompanyApi;
using CompanyApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompanyApiTest
{
    public class CompanyTest
    {
        private readonly TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        private readonly HttpClient client;
        public CompanyTest()
        {
            client = server.CreateClient();
            client.DeleteAsync("companies");
        }

        // companies
        [Fact]
        public async void AC1_should_return_company_when_add_company()
        {
            // given
            var company = new Company("Name1", "0");
            var request = JsonConvert.SerializeObject(company);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var response = await client.PostAsync("companies", requestBody);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<Company>(responseString);

            Assert.Equal(company, actual);
        }

        // companies
        [Fact]
        public async void AC2_should_return_all_companies_when_get_companies()
        {
            // given
            var companies = await AddCompanies();

            // when
            var response = await client.GetAsync("companies");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<List<Company>>(responseString);

            Assert.Equal(companies, actual);
        }

        // companies/{companyId}
        [Fact]
        public async void AC3_should_return_an_existing_company()
        {
            // given
            var companies = await AddCompanies();

            // when
            var response = await client.GetAsync($"companies/{companies[0].CompanyId}");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<Company>(responseString);

            Assert.Equal(companies[0], actual);
        }

        private async Task<List<Company>> AddCompanies()
        {
            var companies = new List<Company>()
            {
                new Company("NAME1", "0"),
                new Company("NAME2", "1"),
                new Company("NAME3", "2"),
            };

            foreach (var requestBody in companies.Select(JsonConvert.SerializeObject)
                .Select(request => new StringContent(request, Encoding.UTF8, "application/json")))
            {
                await client.PostAsync("companies", requestBody);
            }

            return companies;
        }
    }
}
