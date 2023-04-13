using HousingProject.Core.Models.mpesaauthvm;
using HousingProject.Infrastructure.CRUDServices.MainPaymentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Authorize]
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


    }
}
