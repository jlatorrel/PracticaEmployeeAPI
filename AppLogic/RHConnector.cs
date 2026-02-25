using System;
using System.Collections.Generic;
using System.Text;
using DTO;
using Newtonsoft.Json;

namespace AppLogic
{
    public interface IRHConnector
    {
        Task<List<Employee>> RetrieveAllEmployees();
        Task<List<string>> RetrieveAllSpecialties();
        Task<Employee?> GetEmployeeManager(int employeeId);
        Task<List<Employee>> GetOldestEmployee();
        Task<List<Employee>> GetNewestEmployee();
        Task<Employee?> GetEmployeeById(int employeeId);
        Task<List<Employee>> GetEmployeesWithMoreThan(int years);
        Task<List<Employee>> GetEmployeesWithLessThan(int years);
    }
    public class RHConnector : IRHConnector
    {
        private static HttpClient _httpClient;
        private const string _baseUrl = "https://rh-central.azurewebsites.net/";

        public RHConnector()
        {
            if (_httpClient is null)
            {
                _httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(_baseUrl),
                    Timeout = TimeSpan.FromSeconds(15)
                };
            }
        }

        public async Task<List<Employee>> RetrieveAllEmployees()
        {
            string serviceUrl = "/api/RH/GetAllEmployees";
            string result = await InvokeGetAsync(serviceUrl);
            var dtoEmployees = JsonConvert.DeserializeObject<List<Employee>>(result);

            return dtoEmployees;
        }
        public async Task<List<string>> RetrieveAllSpecialties()
        {
            string serviceUrl = "/api/RH/GetSpecialties";
            string result = await InvokeGetAsync(serviceUrl);
            var specialtiesStrings = JsonConvert.DeserializeObject<List<string>>(result);

            return specialtiesStrings;
        }

        #region Metodos Helpers
        private async Task<string> InvokeGetAsync(string uri)
        {
            try
            {
                string responseString = string.Empty;
                var results = await _httpClient.GetAsync(uri);
                if (results.IsSuccessStatusCode)
                {
                    responseString = await results.Content.ReadAsStringAsync();
                }

                return responseString;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Employee?> GetEmployeeManager(int employeeId)
        {
            var employees = await RetrieveAllEmployees();
            var employee = employees.FirstOrDefault(e => e.Id == employeeId);

            if (employee == null || employee.ManagerId == null)
                return null;

            return employees.FirstOrDefault(e => e.Id == employee.ManagerId);
        }

        public async Task<List<Employee>> GetOldestEmployee()
        {
            var employees = await RetrieveAllEmployees();
            var minHireDate = employees.Min(e => e.HiringDate);

            return employees
                .Where(e => e.HiringDate == minHireDate)
                .ToList();
        }

        public async Task<List<Employee>> GetNewestEmployee()
        {
            var employees = await RetrieveAllEmployees();
            var maxHireDate = employees.Max(e => e.HiringDate);

            return employees
                .Where(e => e.HiringDate == maxHireDate)
                .ToList();
        }

        public async Task<Employee?> GetEmployeeById(int employeeId)
        {
            var employees = await RetrieveAllEmployees();
            return employees.FirstOrDefault(e => e.Id == employeeId);
        }

        public async Task<List<Employee>> GetEmployeesWithMoreThan(int years)
        {
            var employees = await RetrieveAllEmployees();
            var cutoffDate = DateTime.Now.AddYears(-years);

            return employees
                .Where(e => e.HiringDate <= cutoffDate)
                .ToList();
        }

        public async Task<List<Employee>> GetEmployeesWithLessThan(int years)
        {
            var employees = await RetrieveAllEmployees();
            var cutoffDate = DateTime.Now.AddYears(-years);

            return employees
                .Where(e => e.HiringDate >= cutoffDate)
                .ToList();
        }

        private async Task<string> InvokePutAsync(string uri, StringContent content)
        {
            try
            {
                string responseString = string.Empty;
                var results = await _httpClient.PutAsync(uri, content);
                if (results.IsSuccessStatusCode)
                {
                    responseString = await results.Content.ReadAsStringAsync();
                }

                return responseString;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async Task<string> InvokePostAsync(string uri, StringContent content)
        {
            try
            {
                string responseString = string.Empty;
                var results = await _httpClient.PostAsync(uri, content);
                if (results.IsSuccessStatusCode)
                {
                    responseString = await results.Content.ReadAsStringAsync();
                }

                return responseString;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion Metodos Helpers
    }
}
