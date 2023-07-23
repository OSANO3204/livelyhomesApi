using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.mpesaauthvm;
using HousingProject.Infrastructure.CRUDServices.MainPaymentServices;
using HousingProject.Infrastructure.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace HousingProject.API.Controllers.Payment
{
    [Route("api/[controller]", Name = "Payment")]
    [ApiController]
    public class PaymentController : IpaymentServices
    {
        private readonly IpaymentServices _paymentServices;
        public PaymentController(IpaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }

        [Route("Get_mpesa_auth_token")]
        [HttpGet]
        public async Task<mpesaAuthenticationvm> Getauthenticationtoken()
        {
            return await _paymentServices.Getauthenticationtoken();
        }


        [Authorize]
        [Route("Register_Urls")]
        [HttpPost]
        public async Task<string> RegisterURL()
        {
            return await _paymentServices.RegisterURL();
        }

        [Route("Stk_Push")]
        [HttpPost]
        public async Task<stk_push_response> STk_Push(string phoneNumber, decimal amount)
        {
            return await _paymentServices.STk_Push(phoneNumber, amount);

        }

        [Route("Get_CallBack_Body")]
        [HttpPost]
        public async Task Get_CallBack_Body(JObject requestBody)
        {
             await _paymentServices.Get_CallBack_Body(requestBody);

        }


        [Route("Add_Confirmation_url")]
        [HttpPost]
        public async Task<string> RegisterConfirmationUrl()
        {
            return await _paymentServices.RegisterConfirmationUrl();

        }

        [Route("Add_validation_url")]
        [HttpPost]
        public async Task<string> RegisterValidationUrl()
        {
            return await _paymentServices.RegisterValidationUrl();
        }

        public Task SendReceipts()
        {
            throw new System.NotImplementedException();
        }
    }
}
