using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using CompanyApi;
using CompanyApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
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
            var company = new Company("Name1");
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
    }
}
