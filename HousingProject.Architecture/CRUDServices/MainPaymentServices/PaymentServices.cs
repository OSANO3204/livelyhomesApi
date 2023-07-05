using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Mpesa;
using HousingProject.Core.Models.mpesaauthvm;
using HousingProject.Core.Models.RentPayment;
using HousingProject.Infrastructure.Interfaces.IUserExtraServices;
using HousingProject.Infrastructure.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HousingProject.Infrastructure.CRUDServices.MainPaymentServices
{

    //daraja v2-https://developer.safaricom.co.ke/APIs/MpesaExpressSimulate
    public class PaymentServices: IpaymentServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private const string DarajaEndpoint = "https://api.safaricom.co.ke";
        private readonly IUserExtraServices _userExtraServices;

        private ILogger<PaymentServices> _logger;
        public PaymentServices(IHttpClientFactory httpClientFactory, 
            IServiceScopeFactory serviceScopeFactory,
              ILogger<PaymentServices> logger,
            IUserExtraServices userExtraServices
            )
        {
            _httpClientFactory = httpClientFactory;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _userExtraServices = userExtraServices;
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
    
            var request = new HttpRequestMessage(HttpMethod.Get, _url);
            request.Headers.Add("Authorization", $"Basic  {base64Auth}");
            var response = await client.SendAsync(request);
            var mpesaauthtoken = await response.Content.ReadAsStringAsync();
            var responseobject = JsonConvert.DeserializeObject<mpesaAuthenticationvm>(mpesaauthtoken);
            return responseobject;
        }

        public async Task<string> RegisterURL()
        {
            var jsonbody = JsonConvert.SerializeObject(new
            {

                ResponseType = "completed",
                ConfirmationURL = "https://webhook.site/eba2eadf-7794-4893-975e-bf1142371919",
                ValidationURL = "https://webhook.site/eba2eadf-7794-4893-975e-bf1142371919",
                Shortcode = "600997",
            });

            var sentbody = new StringContent(

                jsonbody.ToString(),
                Encoding.UTF8,
                "application/json");

            var token = Getauthenticationtoken().Result.access_token;
            var client = _httpClientFactory.CreateClient("mpesa");

            client.DefaultRequestHeaders.Add($"Authorization", $"Bearer {token}");
            var _url = "/mpesa/c2b/v1/registerurl";

            var response = await client.PostAsync(_url, sentbody);

            return await response.Content.ReadAsStringAsync();


        }
        public async Task<stk_push_response> STk_Push( string phoneNumber, decimal amount)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();


                    var trans_Reference = GetGeneratedref().Result;
                    string transactionDesc = "C2b Transactions";
                    var accessToken = Getauthenticationtoken().Result;
                    var client = _httpClientFactory.CreateClient("mpesa");
                    var shortcode = "174379";
                    var passkey = "bfb279f9aa9bdbcf158e97dd71a467cd2e0c893059b10f78e6b72ada1ed2c919";

                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var encorded_pass = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{shortcode}{passkey}{timestamp}"));

                    var requestBody = new
                    {
                        BusinessShortCode = shortcode,
                        Password = encorded_pass,
                        Timestamp = timestamp,
                        TransactionType = "CustomerPayBillOnline",
                        Amount = amount.ToString(),
                        PartyA = phoneNumber,
                        PartyB = shortcode,
                        PhoneNumber = phoneNumber,
                        CallBackURL = "https://webhook.site/d0e6975c-22b2-411d-a793-a4bc1d06b5c0",
                        AccountReference = trans_Reference,
                        TransactionDesc = transactionDesc
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer " + accessToken.access_token);
                    string _url = "/mpesa/stkpush/v1/processrequest";
                    var response = await client.PostAsync(_url, content);


                    var responseContent = await response.Content.ReadAsStringAsync();

                    var json_resp_body = JsonConvert.DeserializeObject<Stk_Push_Response_Body>(responseContent);




                    //public string CustomerMessage { get; set; }

                    var new_response_body = new Stk_Push_Response_Body
                    {

                        MerchantRequestID = json_resp_body.MerchantRequestID,
                        CheckoutRequestID = json_resp_body.CheckoutRequestID,
                        ResponseCode = json_resp_body.ResponseCode,
                        ResponseDescription = json_resp_body.ResponseDescription,
                        CustomerMessage = json_resp_body.CustomerMessage,
                        ReferenceNumber=requestBody.AccountReference

                    };

                    await scopedcontext.AddAsync(new_response_body);
                   var saved_resp=  await scopedcontext.SaveChangesAsync();
                   
                    Console.WriteLine(responseContent);


                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("STK push initiated successfully.");
                        return new stk_push_response { Code = "200", internalref = trans_Reference };
                    }
                    else
                    {
                        Console.WriteLine("Failed to initiate STK push.");
                        return new stk_push_response { Code = "350", internalref = trans_Reference };
                    }

                }
            }
            catch (Exception ex)
            {
                return new stk_push_response { Code = "367", message=ex.Message };
            }
        
        }

        public async Task<string> GetGeneratedref()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    int length = 11;
                    var paymentref = "LH_" + _userExtraServices.GenerateReferenceNumber(length);
                    //check reference exists
                    var referenceexists = await scopedcontext.PayRent.Where(y => y.InternalReference == paymentref).ToListAsync();
                    if (referenceexists.Count >= 1)
                    {
                        await GetGeneratedref();
                    }
                    return paymentref;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
        public async Task<BaseResponse> Get_CallBack_Body([FromBody] JObject requestBody)
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var callbackData = JsonConvert.SerializeObject(requestBody);

                    var stk_body = JsonConvert.DeserializeObject<STKCallback>(callbackData);

                    var body_object = stk_body.Body.stkCallback;

                    var new_callback = new Save_Callback_Body
                    {


                        //public int ResultCode { get; set; }
                        //public string ResultDesc { get; set; }
                        MerchantRequestID = body_object.MerchantRequestID,
                        CheckoutRequestID = body_object.CheckoutRequestID,
                        ResultCode = body_object.ResultCode,
                        ResultDesc = body_object.ResultDesc

                    };

                    await scopedcontext.AddAsync(new_callback);
                    await scopedcontext.SaveChangesAsync();

                        return new BaseResponse { Code = "200", SuccessMessage = " saved successfull", Body = body_object };

                   

                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("_______" + ex.Message + " ____|||||______");
                return new BaseResponse { Code = "170", SuccessMessage = ex.Message, Body = ex.StackTrace };
            }


        }

       
    }
}
