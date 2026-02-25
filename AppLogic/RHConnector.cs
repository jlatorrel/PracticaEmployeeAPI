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
