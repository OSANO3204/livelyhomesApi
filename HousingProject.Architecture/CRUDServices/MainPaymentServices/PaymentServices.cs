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

namespace HousingProject.Infrastructure.CRUDServices.MainPaymentServices
{
   public  class PaymentServices: IpaymentServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public PaymentServices(IHttpClientFactory httpClientFactory, IServiceScopeFactory serviceScopeFactory)
        {
            _httpClientFactory = httpClientFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<mpesaAuthenticationvm> Getauthenticationtoken()
        {
            var client = _httpClientFactory.CreateClient("mpesa");
            var authString = "ayDShXXoHYSaZXCkeNXM7G8318udLh3V:uike4BHVsClxdvTJ";
            var encodedpassword = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authString));
            var _url = "/oauth/v1/generate?grant_type=client_credentials";
            var request = new HttpRequestMessage(HttpMethod.Get, _url);
            request.Headers.Add("Authorization", $"Basic {encodedpassword}");
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
        //public async Task<object> mpesa_validationUrl()
        //{
        //    try
        //    {
        //        using (var scope = _serviceScopeFactory.CreateScope())
        //        {
        //            var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new { ex.Message };
        //    }
        //}
    }
}
