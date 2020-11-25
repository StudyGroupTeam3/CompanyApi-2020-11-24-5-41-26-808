using CompanyApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        [Fact]
        public async Task Should_Return_All_Companies_When_Get_All_Companies()
        {
            // given
            List<Company> companyList = new List<Company> { new Company(name: "Apple"), new Company(name: "Google") };
            await client.DeleteAsync("Company/clear");
            foreach (var company in companyList)
            {
                string request = JsonConvert.SerializeObject(company);
                StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
                await client.PostAsync("Company/companies", requestBody);
            }

            // when
            var response = await client.GetAsync("Company/companies");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Company> actualCompanies = JsonConvert.DeserializeObject<List<Company>>(responseString);

            // then
            Assert.Equal(companyList, actualCompanies);
        }

        [Fact]
        public async Task Should_Return_Correct_Company_When_Get_Specific_Company()
        {
            // given
            List<Company> companyList = new List<Company> { new Company(name: "Apple"), new Company(name: "Google") };
            await client.DeleteAsync("Company/clear");
            foreach (var company in companyList)
            {
                string request = JsonConvert.SerializeObject(company);
                StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
                await client.PostAsync("Company/companies", requestBody);
            }

            // when
            var response = await client.GetAsync($"Company/companies/{companyList[0].Id}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actualCompany = JsonConvert.DeserializeObject<Company>(responseString);

            // then
            Assert.Equal(companyList[0], actualCompany);
        }

        [Fact]
        public async Task Should_Return_Correct_Companies_In_One_Page_When_Get_Companies_In_Page()
        {
            // given
            List<Company> companyList = new List<Company> { new Company(name: "Apple"), new Company(name: "Google"), new Company(name: "Egypt"), new Company(name: "India") };
            await client.DeleteAsync("Company/clear");
            foreach (var company in companyList)
            {
                string request = JsonConvert.SerializeObject(company);
                StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
                await client.PostAsync("Company/companies", requestBody);
            }

            // when
            var response = await client.GetAsync($"Company/companies/pages/2?pageSize=2");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Company> actualCompany = JsonConvert.DeserializeObject<List<Company>>(responseString);

            // then
            Assert.Equal(new List<Company>() { companyList[2], companyList[3] }, actualCompany);
        }

        [Fact]
        public async Task Should_Update_Basic_Company_Information_When_Update_Company()
        {
            // given
            await client.DeleteAsync("Company/clear");
            Company company = new Company(name: "Apple");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("Company/companies", requestBody);

            // when
            UpdateCompany updateCompany = new UpdateCompany("Pineapple");
            string putRequest = JsonConvert.SerializeObject(updateCompany);
            StringContent putRequestBody = new StringContent(putRequest, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"Company/companies/{company.Id}", putRequestBody);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actualCompany = JsonConvert.DeserializeObject<Company>(responseString);

            // then
            Assert.Equal(new Company("Pineapple") { Id = company.Id }, actualCompany);
        }

        [Fact]
        public async Task Should_Add_Employee_To_Specific_Company_When_Add_Employee_To_Specific_Company()
        {
            // given
            await client.DeleteAsync("Company/clear");
            Company company = new Company(name: "Apple");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("Company/companies", requestBody);

            // when
            Employee employee = new Employee("Jack", 12000);
            string patchRequest = JsonConvert.SerializeObject(employee);
            StringContent putRequestBody = new StringContent(patchRequest, Encoding.UTF8, "application/json");
            var response = await client.PatchAsync($"Company/companies/{company.Id}/employees", putRequestBody);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Employee> actualEmployees = JsonConvert.DeserializeObject<List<Employee>>(responseString);

            // then
            Assert.Equal(new List<Employee> { employee }, actualEmployees);
        }

        [Fact]
        public async Task Should_Return_Employees_In_Specific_Company_When_Get_Employees_In_Specific_Company()
        {
            // given
            await client.DeleteAsync("Company/clear");
            Employee employee = new Employee("Jack", 12000);
            Company company = new Company(name: "Apple") { Employees = new List<Employee> { employee } };
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("Company/companies", requestBody);

            // when
            var response = await client.GetAsync($"Company/companies/{company.Id}/employees");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Employee> actualEmployees = JsonConvert.DeserializeObject<List<Employee>>(responseString);

            // then
            Assert.Equal(new List<Employee> { employee }, actualEmployees);
        }

        [Fact]
        public async Task Should_Update_Employee_In_Specific_Company_When_Update_Employee_In_Specific_Company()
        {
            // given
            await client.DeleteAsync("Company/clear");
            Employee employee = new Employee("Jack", 12000);
            Company company = new Company(name: "Apple") { Employees = new List<Employee> { employee } };
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("Company/companies", requestBody);

            // when
            UpdateEmployee updateEmployee = new UpdateEmployee("Rose", 30000);
            string patchRequest = JsonConvert.SerializeObject(updateEmployee);
            StringContent patchRequestBody = new StringContent(patchRequest, Encoding.UTF8, "application/json");
            var response = await client.PatchAsync($"Company/companies/{company.Id}/{employee.Id}", patchRequestBody);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Employee actualEmployee = JsonConvert.DeserializeObject<Employee>(responseString);

            // then
            Assert.Equal(new Employee("Rose", 30000) { Id = employee.Id }, actualEmployee);
        }

        [Fact]
        public async Task Should_Delete_Specific_Employee_In_Specific_Company_When_Delete_Employee_In_Specific_Company()
        {
            // given
            await client.DeleteAsync("Company/clear");
            Employee employee = new Employee("Jack", 12000);
            Company company = new Company(name: "Apple") { Employees = new List<Employee> { employee } };
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("Company/companies", requestBody);

            // when
            UpdateEmployee updateEmployee = new UpdateEmployee("Rose", 30000);
            string patchRequest = JsonConvert.SerializeObject(updateEmployee);
            StringContent patchRequestBody = new StringContent(patchRequest, Encoding.UTF8, "application/json");
            await client.DeleteAsync($"Company/companies/{company.Id}/{employee.Id}");
            var response = await client.GetAsync($"Company/companies/{company.Id}/employees");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Employee> actualEmployees = JsonConvert.DeserializeObject<List<Employee>>(responseString);

            // then
            Assert.Null(actualEmployees.Find(thisCompany => thisCompany.Id == company.Id)?.ToString());
        }
    }
}