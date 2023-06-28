using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using HousingProject.Core.Models.mpesaauthvm;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using HousingProject.Architecture.Data;
using System.Web.Http.Results;
using System.Net.Http.Headers;
using HousingProject.Architecture.Interfaces.IRenteeServices;

namespace HousingProject.Infrastructure.CRUDServices.MainPaymentServices
{
   public  class PaymentServices: IpaymentServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private ITenantServices _tenant_services;
        public PaymentServices(IHttpClientFactory httpClientFactory, 
            IServiceScopeFactory serviceScopeFactory,
            ITenantServices tenant_services
            )
        {
            _httpClientFactory = httpClientFactory;
            _serviceScopeFactory = serviceScopeFactory;
            _tenant_services = tenant_services;
        }

        public async Task<mpesaAuthenticationvm> Getauthenticationtoken()
        {
        
            var client = _httpClientFactory.CreateClient("mpesa");

            string username = "gRO5xyvopGEiUrkMoN43m3OjQfn0b1TY";
            string password = "FQSx1olBTIbcJxEb";
            string auth = $"{username}:{password}";
            byte[] authBytes = Encoding.ASCII.GetBytes(auth);
            string base64Auth = Convert.ToBase64String(authBytes);  
            var _url = "/oauth/v1/generate?grant_type=client_credentials";
           // var fullauth = "https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials";
            var request = new HttpRequestMessage(HttpMethod.Get, _url);
            request.Headers.Add("Authorization", $"Basic  {base64Auth}");
            var response = await client.SendAsync(request);
            var mpesaauthtoken = await response.Content.ReadAsStringAsync();
            var responseobject = JsonConvert.DeserializeObject<mpesaAuthenticationvm>(mpesaauthtoken);
            return responseobject;
        }

        public async Task<string> RegisterURL()
        {
            var jsonbody = JsonConvert.SerializeObject(new {
           
                ResponseType="completed",
                ConfirmationURL= "https://webhook.site/eba2eadf-7794-4893-975e-bf1142371919",
                ValidationURL= "https://webhook.site/eba2eadf-7794-4893-975e-bf1142371919",
                Shortcode = "600997",
            });

            var sentbody = new StringContent(

                jsonbody.ToString(),
                Encoding.UTF8,
                "application/json");

            var token =  Getauthenticationtoken().Result.access_token;
            var client = _httpClientFactory.CreateClient("mpesa");

            client.DefaultRequestHeaders.Add($"Authorization", $"Bearer {token}");
            var _url = "/mpesa/c2b/v1/registerurl";

            var response = await client.PostAsync(_url, sentbody);

            return await  response.Content.ReadAsStringAsync();
          

        }
        public async Task<string> STk_Push( string phoneNumber, decimal amount)
        {
            var accountReference = _tenant_services.GetGeneratedref().Result;
            string transactionDesc = "C2b Transactions";
            var accessToken= Getauthenticationtoken().Result;
            var client = _httpClientFactory.CreateClient("mpesa");

            var request = new HttpRequestMessage(HttpMethod.Post, "stkpush/v1/processrequest"); 

            request.Headers.Add("Authorization", $"Bearer {accessToken.access_token}");

            // Set the request payload
            var payload = new Dictionary<string, string>
        {
            
            { "Timestamp", DateTime.Now.ToString("yyyyMMddHHmmss") },
            { "TransactionType", "CustomerPayBillOnline" },
            { "Amount", amount.ToString("F2") },
            { "PartyA", phoneNumber },
            { "PhoneNumber", phoneNumber },
            { "CallBackURL", "https://webhook.site/38ab2f23-57d0-420a-977a-e1cd34f1f12f" }, // Replace with your callback URL
            { "AccountReference", accountReference },
            { "TransactionDesc", transactionDesc }
        };
            request.Content = new FormUrlEncodedContent(new[]
                   {
                   new KeyValuePair<string, string>("grant_type", "client_credentials")
                  });

            request.Content = new FormUrlEncodedContent(payload);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                // Handle error response
                return null;
            }
        }
    }
}
