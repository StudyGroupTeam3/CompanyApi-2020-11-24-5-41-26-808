using System;
using System.Collections.Generic;
using System.Linq;
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
            Assert.Equal("http://localhost/Companies/0", response.Headers.Location.ToString());
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
            // then
            Assert.Equal(409, (int)secondResponse.StatusCode);
        }

        [Fact]
        public async Task Should_Return_All_Company_List_When_Get_GetAllCompany()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("companies/clear");
            Company company = new Company(null, "Baymax");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("/companies", requestBody);
            // when
            var response = await client.GetAsync("companies");
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualCompanies = JsonConvert.DeserializeObject<List<Company>>(responseString);
            Assert.Equal(new List<Company>() { new Company("0", "Baymax") }, actualCompanies);
        }

        [Fact]
        public async Task Should_Return_Specified_Company_When_Get_GetCompanyByID()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("companies/clear");
            Company company = new Company(null, "Baymax");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("/companies", requestBody);
            // when
            var response = await client.GetAsync("companies/0");
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualCompanies = JsonConvert.DeserializeObject<Company>(responseString);
            Assert.Equal(new Company("0", "Baymax"), actualCompanies);
        }

        [Fact]
        public async Task Should_Return_Page_Size_Company_From_StartPage_When_Get_GetCompanyByPage()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("companies/clear");
            Company company1 = new Company("0", "Baymax");
            string request1 = JsonConvert.SerializeObject(company1);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.PostAsync("/companies", requestBody1);
            Company company2 = new Company("1", "Jack");
            string request2 = JsonConvert.SerializeObject(company2);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("/companies", requestBody2);
            Company company3 = new Company("2", "IBM");
            string request3 = JsonConvert.SerializeObject(company3);
            StringContent requestBody3 = new StringContent(request3, Encoding.UTF8, "application/json");
            await client.PostAsync("/companies", requestBody3);
            // when
            var response = await client.GetAsync("companies/page?pagesize=2&startpage=2");
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualCompanies = JsonConvert.DeserializeObject<List<Company>>(responseString);
            Assert.Equal(new List<Company>() { company3 }, actualCompanies);
        }

        [Fact]
        public async Task Should_Update_Company_Property_When_Patch_UpdateCompany()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("companies/clear");
            Company company1 = new Company("0", "Baymax");
            string request1 = JsonConvert.SerializeObject(company1);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.PostAsync("/companies", requestBody1);
            // when
            CompanyUpdateModel companyUpdateModel = new CompanyUpdateModel("GE");
            string requestPatch = JsonConvert.SerializeObject(companyUpdateModel);
            StringContent requestPatchBody = new StringContent(requestPatch, Encoding.UTF8, "application/json");
            var response = await client.PatchAsync("companies/0", requestPatchBody);
            // then
            response.EnsureSuccessStatusCode();
            var getResponse = await client.GetAsync("companies/0");
            var getResponseString = await getResponse.Content.ReadAsStringAsync();
            var actualCompany = JsonConvert.DeserializeObject<Company>(getResponseString);
            Assert.Equal(new Company("0", "GE"), actualCompany);
        }

        [Fact]
        public async Task Should_Created_New_Employee_When_Post_AddEmployee()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("companies/clear");
            Company company1 = new Company("0", "Baymax");
            string request1 = JsonConvert.SerializeObject(company1);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.PostAsync("/companies", requestBody1);
            // when
            Employee newEmployee = new Employee("0", "Jim", 10000);
            string request = JsonConvert.SerializeObject(newEmployee);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/companies/0/employees", requestBody);
            // then
            response.EnsureSuccessStatusCode();
            var getResponse = await client.GetAsync("companies/0");
            var getResponseString = await getResponse.Content.ReadAsStringAsync();
            var actualCompany = JsonConvert.DeserializeObject<Company>(getResponseString);
            Assert.Equal(new Employee("0", "Jim", 10000), actualCompany.Employees["0"]);
        }

        [Fact]
        public async Task Should_Return_All_Employee_When_Get_GetAllEmployee()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("companies/clear");
            Company company1 = new Company("0", "Baymax");
            Employee newEmployee = new Employee("0", "Jim", 10000);
            company1.Employees["0"] = newEmployee;
            string request1 = JsonConvert.SerializeObject(company1);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.PostAsync("/companies", requestBody1);
            // when
            var getResponse = await client.GetAsync("companies/0/employees");
            var getResponseString = await getResponse.Content.ReadAsStringAsync();
            var allEmployees = JsonConvert.DeserializeObject<List<Employee>>(getResponseString);
            // then
            Assert.Equal(new List<Employee>() { newEmployee }, allEmployees);
        }

        [Fact]
        public async Task Should_Change_Specified_Employee_In_Specified_Company_When_Patch_UpdateEmployee()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("companies/clear");
            Company company1 = new Company("0", "Baymax");
            Employee newEmployee = new Employee("0", "Jim", 10000);
            company1.Employees["0"] = newEmployee;
            string request1 = JsonConvert.SerializeObject(company1);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.PostAsync("/companies", requestBody1);
            // when
            var employeeUpdateModel = new EmployeeUpdateModel("Jim", 12000);
            string request = JsonConvert.SerializeObject(employeeUpdateModel);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PatchAsync("/companies/0/employees/0", requestBody);
            // then
            var getResponse = await client.GetAsync("companies/0");
            var getResponseString = await getResponse.Content.ReadAsStringAsync();
            var actualCompany = JsonConvert.DeserializeObject<Company>(getResponseString);
            var expectedEmployee = new Employee("0", "Jim", 12000);
            Assert.Equal(expectedEmployee, actualCompany.Employees["0"]);
        }
    }
}
