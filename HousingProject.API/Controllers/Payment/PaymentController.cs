using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.mpesaauthvm;
using HousingProject.Core.ViewModel.Payment;
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

        [Authorize]
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

        [Authorize]
        [Route("Add_Confirmation_url")]
        [HttpPost]
        public async Task<string> RegisterConfirmationUrl()
        {
            return await _paymentServices.RegisterConfirmationUrl();
        }

        [Authorize]
        [Route("Add_validation_url")]
        [HttpPost]
        public async Task<string> RegisterValidationUrl()
        {
            return await _paymentServices.RegisterValidationUrl();
        }

        [Authorize]
        [Route("send_receipts")]
        [HttpPost]
        public Task SendReceipts()
        {
            throw new System.NotImplementedException();
        }

        [Authorize]
        [Route("Paginated_Transactions")]
        [HttpPost]
        public async Task<BaseResponse> GetPaginatedTransactions(int pageNumber)
        {
            return await _paymentServices.GetPaginatedTransactions(pageNumber);
        }

        [Authorize]
        [Route("Payment_setup")]
        [HttpPost]
        public async Task<BaseResponse> SetUp_Payment(paymentCodesvm vm)
        {
            return await _paymentServices.SetUp_Payment(vm);
        }


        [Authorize]
        [Route("Payment_info_update")]
        [HttpPost]
        public async Task<BaseResponse> update_payment_setup(paymentCodesvm vm)
        {
            return await _paymentServices.update_payment_setup(vm);
        }

        [Authorize]
        [Route("payment_info_by_houseid")]
        [HttpPost]
        public async Task<BaseResponse> GetPaymentInfByHouseid(int houseid)
        {
            return await _paymentServices.GetPaymentInfByHouseid(houseid);
        }
    }
}
