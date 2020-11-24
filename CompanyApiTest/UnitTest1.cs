using System;
using System.Collections.Generic;
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
            var response1 = await client.GetAsync("CompanyApi/companies? name = Tecent");
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
    }
}
