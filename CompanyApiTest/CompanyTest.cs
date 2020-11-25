﻿using CompanyApi;
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
        public async void AC3_should_return_an_existing_company_when_get_existing_company()
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

        // companies/{companyID}
        [Fact]
        public async void AC5_should_update_information_of_existing_company_when_update()
        {
            // given
            var companies = await AddCompanies();
            var updateData = new Update("newNAME");

            // when
            var request = JsonConvert.SerializeObject(updateData);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PatchAsync($"companies/{companies[0].CompanyId}", requestBody);
            var response = await client.GetAsync($"companies/{companies[0].CompanyId}");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<Company>(responseString);

            Assert.Equal(updateData.Name, actual.Name);
        }

        // companies/{companyID}/employees
        [Fact]
        public async void AC6_should_return_employee_when_add_employee_in_specific_company()
        {
            // given
            var companies = await AddCompanies();
            var employee = new Employee("0", "person1", 1000);

            // when
            var request = JsonConvert.SerializeObject(employee);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"companies/{companies[0].CompanyId}/employees", requestBody);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<Employee>(responseString);

            Assert.Equal(employee, actual);
        }

        // companies/{companyID}/employees
        [Fact]
        public async void AC7_should_return_employee_list_when_get_all_employees_of_company()
        {
            // given
            var companies = await AddCompanies();
            var employees = new List<Employee>()
            {
                new Employee("0", "person1", 1000),
                new Employee("1", "person2", 2000),
                new Employee("2", "person3", 3000),
            };

            // when
            foreach (var requestBody in employees.Select(JsonConvert.SerializeObject)
                .Select(request => new StringContent(request, Encoding.UTF8, "application/json")))
            {
                await client.PostAsync($"companies/{companies[0].CompanyId}/employees", requestBody);
            }

            var response = await client.GetAsync($"companies/{companies[0].CompanyId}/employees");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<List<Employee>>(responseString);

            Assert.Equal(employees, actual);
        }

        // companies/{companyID}/employees/{employeeID}
        [Fact]
        public async void AC8_should_update_information_of_employee_under_specific_company()
        {
            // given
            var companies = await AddCompanies();
            var employees = new List<Employee>()
            {
                new Employee("0", "person1", 1000),
                new Employee("1", "person2", 2000),
                new Employee("2", "person3", 3000),
            };
            var updateData = new Update("newNAME");

            // when
            foreach (var requestBody in employees.Select(JsonConvert.SerializeObject)
                .Select(request => new StringContent(request, Encoding.UTF8, "application/json")))
            {
                await client.PostAsync($"companies/{companies[0].CompanyId}/employees", requestBody);
            }

            var request2 = JsonConvert.SerializeObject(updateData);
            var requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PatchAsync($"companies/{companies[0].CompanyId}/employees/{employees[2].EmployeeID}", requestBody2);
            var response = await client.GetAsync($"companies/{companies[0].CompanyId}/employees");
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<List<Employee>>(responseString);

            Assert.Equal(updateData.Name, actual[2].Name);
        }

        // companies/{companyID}/employees/{employeeID}
        [Fact]
        public async void AC9_should_delete_employee_under_specific_company()
        {
            // given
            var companies = await AddCompanies();
            var employees = new List<Employee>()
            {
                new Employee("0", "person1", 1000),
                new Employee("1", "person2", 2000),
                new Employee("2", "person3", 3000),
            };

            // when
            foreach (var requestBody in employees.Select(JsonConvert.SerializeObject)
                .Select(request => new StringContent(request, Encoding.UTF8, "application/json")))
            {
                await client.PostAsync($"companies/{companies[0].CompanyId}/employees", requestBody);
            }

            await client.DeleteAsync($"companies/{companies[0].CompanyId}/employees/{employees[2].EmployeeID}");
            employees.Remove(employees[2]);
            var response = await client.GetAsync($"companies/{companies[0].CompanyId}/employees");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<List<Employee>>(responseString);

            Assert.Equal(employees, actual);
        }

        // companies/{companyID}
        [Fact]
        public async void AC10_should_return_empty_employee_list_when_delete_specific_company()
        {
            // given
            var companies = await AddCompanies();

            // when
            await client.DeleteAsync($"companies/{companies[2].CompanyId}");
            var response = await client.GetAsync("companies");
            companies.Remove(companies[2]);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<List<Company>>(responseString);

            Assert.Equal(companies, actual);
        }

        // companies?pageSize={x}&&pageIndex={y}
        [Fact]
        public async void AC4_should_return_correct_companies_when_get_X_companies_from_page_index_Y()
        {
            // given
            var companies = await Add30Companies();

            // when
            const int pageSize = 6;
            const int pageIndex = 3;
            var response = await client.GetAsync($"companies?pageSize={pageSize}&pageIndex={pageIndex}");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<List<Company>>(responseString);

            Assert.Equal(companies.GetRange(12, 6), actual);
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

        private async Task<List<Company>> Add30Companies()
        {
            var companies = new List<Company>();
            for (var i = 0; i < 30; i++)
            {
                companies.Add(new Company($"NAME{i}", i.ToString()));
            }

            foreach (var requestBody in companies.Select(JsonConvert.SerializeObject)
                .Select(request => new StringContent(request, Encoding.UTF8, "application/json")))
            {
                await client.PostAsync("companies", requestBody);
            }

            return companies;
        }
    }
}
