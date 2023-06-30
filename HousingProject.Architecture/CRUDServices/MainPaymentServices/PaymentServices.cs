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
        private const string DarajaEndpoint = "https://api.safaricom.co.ke";
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

            string username = "ozma4Oaf44ZPkkU6JvMqDpo9VNOb50Oz";
            string password = "ok0vxpMbWCQT6baC";
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

            try
            {

                var accountReference = _tenant_services.GetGeneratedref().Result;
                string transactionDesc = "C2b Transactions";
                var accessToken = Getauthenticationtoken().Result;
                var client = _httpClientFactory.CreateClient("mpesa");
                var shortcode = "999880";
                var request = new HttpRequestMessage(HttpMethod.Post, "stkpush/v1/processrequest");
                request.Headers.Add("Authorization", $"Basic {accessToken.access_token}");
                //shorcode
                var passkey = "ok0vxpMbWCQT6baC";

                   var encoded_timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); 
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.access_token}");
                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                    // Generate the password by base64 encoding the BusinessShortCode, Passkey, and timestamp
                    var encorded_pass = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{shortcode}{passkey}{encoded_timestamp}"));

                    // Prepare the request payload
                    var payload = new

                    {

                        BusinessShortCode = shortcode,
                        Password = encorded_pass,
                        Timestamp = encoded_timestamp,
                        TransactionType = "CustomerPayBillOnline",
                        Amount = amount.ToString(),
                        PartyA = phoneNumber,
                        PartyB = shortcode,
                        PhoneNumber = phoneNumber,
                        CallBackURL = "https://webhook.site/38ab2f23-57d0-420a-977a-e1cd34f1f12f",
                        AccountReference = accountReference + "2675",
                        TransactionDesc = transactionDesc

                    };

                    var requestUri = "/mpesa/stkpush/v1/processrequest";
                    var content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
                     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{accessToken.access_token}");
                    // Send the HTTP POST request to initiate STK push
                    var response = await client.PostAsync(requestUri, payload);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Process the response
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("STK push initiated successfully.");
                        return ("Response: " + responseContent);
                    }
                    else
                    {
                        Console.WriteLine("Failed to initiate STK push.");
                        return ("Response: " + responseContent);
                    }
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        
        }
    }
}
