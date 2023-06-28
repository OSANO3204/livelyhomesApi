using HousingProject.Core.ViewModel.Daraja_vm;
using HousingProject.Infrastructure.Interfaces.IDarraja;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.CRUDServices.Payments.Daraja
{
   public class Daraja_Services: IDarajaServices
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.safaricom.co.ke";

        public Daraja_Services()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetAccessToken(string username, string password)
        {
            var requestBody = new { username, password };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/oauth/v1/generate?grant_type=client_credentials", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error response
                throw new Exception("Failed to retrieve access token.");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<darajavm>(responseContent);

            return tokenResponse.AccessToken;
        }
        public async Task<string> FetchAccessToken()
        {
            var username = "brian.otieno@ngaocredit.com";
            var password = "Kyla@2018";

           
            var accessToken = await GetAccessToken(username, password);

            return accessToken;
        }

    }
}
