using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Extras;
using HousingProject.Core.Models.Mpesa;
using HousingProject.Core.Models.mpesaauthvm;
using HousingProject.Core.Models.RentMonthly;
using HousingProject.Core.Models.RentPayment;
using HousingProject.Core.ViewModel.Rentpayment;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
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
    public class PaymentServices : IpaymentServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILoggedIn _logged_in;
        private const string DarajaEndpoint = "https://api.safaricom.co.ke";
        private readonly IUserExtraServices _userExtraServices;

        private ILogger<PaymentServices> _logger;
        public PaymentServices(IHttpClientFactory httpClientFactory,
            IServiceScopeFactory serviceScopeFactory,
              ILogger<PaymentServices> logger,
              ILoggedIn logged_in,

            IUserExtraServices userExtraServices
            )
        {
            _httpClientFactory = httpClientFactory;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _userExtraServices = userExtraServices;
            _logged_in = logged_in;
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

        public async Task<string> Get_Tenant_Payment_Ref()
        {

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var _user = _logged_in.LoggedInUser().Result;
                    var tenant_exists = await scopedcontext.TenantClass.Where(y => y.Email == _user.Email).FirstOrDefaultAsync();


                    var house_exists = await scopedcontext.House_Registration.Where(y => y.HouseiD == tenant_exists.HouseiD).FirstOrDefaultAsync();


                    var tenant_payment_ref = "LH_" + house_exists.House_Name + "_" + tenant_exists.FirstName.Trim() + "_" + tenant_exists.Appartment_DoorNumber;

                    return tenant_payment_ref;
                }
                catch (Exception ex)
                {

                    return $"an error occured : ERROR DESCRIPIION=>  {ex.Message}";
                }
            }


        }
        public async Task<stk_push_response> STk_Push(string phoneNumber, decimal amount)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    // var trans_Reference = GetGeneratedref().Result;
                    var trans_Reference = Get_Tenant_Payment_Ref().Result;
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
                        CallBackURL = "https://webhook.site/94c3d416-42ba-4919-9cbb-16c7ffda9c63",
                        AccountReference = trans_Reference,
                        TransactionDesc = transactionDesc
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer " + accessToken.access_token);
                    string _url = "/mpesa/stkpush/v1/processrequest";
                    var response = await client.PostAsync(_url, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(responseContent);
                    var json_resp_body = JsonConvert.DeserializeObject<Stk_Push_Response_Body>(responseContent);
                    var new_response_body = new Stk_Push_Response_Body
                    {
                        MerchantRequestID = json_resp_body.MerchantRequestID,
                        CheckoutRequestID = json_resp_body.CheckoutRequestID,
                        ResponseCode = json_resp_body.ResponseCode,
                        ResponseDescription = json_resp_body.ResponseDescription,
                        CustomerMessage = json_resp_body.CustomerMessage,
                        ReferenceNumber = requestBody.AccountReference
                    };

                    await scopedcontext.AddAsync(new_response_body);
                    var saved_resp = await scopedcontext.SaveChangesAsync();
                    Console.WriteLine(responseContent);
                    _logger.LogInformation($"_______________________Logging information ______________________||||________{responseContent}");
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Rent payment initiated SUCCESSFULLY");
                        return new stk_push_response { Code = "200", internalref = trans_Reference, message = new_response_body.MerchantRequestID };
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
                return new stk_push_response { Code = "367", message = ex.Message };
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

                    var generated_number = Adding_Number().Result;
                    var paymentref = "LH_" + _userExtraServices.GenerateReferenceNumber(length) + generated_number;
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
        public async Task Get_CallBack_Body([FromBody] JObject requestBody)
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

                        MerchantRequestID = body_object.MerchantRequestID,
                        CheckoutRequestID = body_object.CheckoutRequestID,
                        ResultCode = body_object.ResultCode,
                        ResultDesc = body_object.ResultDesc

                    };

                    var check_checkoutRequestedID = await scopedcontext.Save_Callback_Body.Where(y => y.MerchantRequestID == new_callback.MerchantRequestID).FirstOrDefaultAsync();
                    if (check_checkoutRequestedID == null)
                    {
                        await scopedcontext.AddAsync(new_callback);
                        await scopedcontext.SaveChangesAsync();

                        _logger.LogInformation("Saved callback message successfully to he database ");

                        //update transaction
                        var check_exists = await scopedcontext.PayRent.Where(u => u.Merchant_Request_ID == new_callback.MerchantRequestID).FirstOrDefaultAsync();

                        if (check_exists != null && new_callback.ResultCode == 0)
                        {

                            check_exists.Status = "COMPLETED";
                            check_exists.ProviderReference = stk_body.Body.stkCallback.CallbackMetadata.Item[1].Value.ToString();
                            scopedcontext.Update(check_exists);
                            await scopedcontext.SaveChangesAsync();

                            var new_update_rent = new update_rent_table
                            {
                                Tenantid = check_exists.TenantId,
                                Paid = (double)check_exists.RentAmount,
                                Provider_Reference = stk_body.Body.stkCallback.CallbackMetadata.Item[1].Value.ToString()
                             };

                            await Update_Monthly_Rent_Payment(new_update_rent);
                             }
                        else
                            {
                                check_exists.Status = "FAILED";
                                check_exists.ProviderReference = "N/A";
                                scopedcontext.Update(check_exists);
                                await scopedcontext.SaveChangesAsync();
                            }

                    }
                    else
                    {
                        _logger.LogInformation("Not saved");
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("_______" + ex.Message + " ____|||||______");
                _logger.LogInformation($"_____||||____________''''''______{ex.Message}");
            }


        }

        public async Task<int> Adding_Number()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();


                    var check_last_number = await scopedcontext.Number_Generator.Where(y => y.Generated_Number > 0).OrderByDescending(u => u.DateUpdated).LastOrDefaultAsync();

                    if (check_last_number == null)
                    {

                        var new_number = new Number_Generator
                        {
                            Generated_Number = 1

                        };

                        await scopedcontext.AddAsync(new_number);
                        await scopedcontext.SaveChangesAsync();
                        _logger.LogInformation("Number added for the first time");
                        return new_number.Generated_Number;

                    }
                    else
                    {
                        var number_update = 1;
                        check_last_number.Generated_Number = check_last_number.Generated_Number + number_update;
                        scopedcontext.Update(check_last_number);
                        await scopedcontext.SaveChangesAsync();

                        _logger.LogInformation("Successfully done");

                        return check_last_number.Generated_Number;
                    }
                }

            }
            catch (Exception ex)
            {
                return ex.Data.Count;

            }
        }

        public async Task Update_Monthly_Rent_Payment(update_rent_table vm)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var tenant_payment_exists = await scopedcontext.Rent_Monthly_Update
                        .Where(y => y.Tenantid == vm.Tenantid).OrderByDescending(y => y.DateCreated).FirstOrDefaultAsync();

                    tenant_payment_exists.Paid = tenant_payment_exists.Paid + vm.Paid;
                    tenant_payment_exists.Provider_Reference = vm.Provider_Reference;
                    tenant_payment_exists.Balance = tenant_payment_exists.Balance-vm.Paid;
                    tenant_payment_exists.DateUpdated = DateTime.Now;
                    scopedcontext.Update(tenant_payment_exists);
                    await scopedcontext.SaveChangesAsync();
                    _logger.LogInformation("__successfully updated rent amount");

                }
            }
            catch (Exception ex)
            {

                _logger.LogInformation("___________||||_______ERROR MESSAGE: ", ex.Message);

            }

        }
    }
}
