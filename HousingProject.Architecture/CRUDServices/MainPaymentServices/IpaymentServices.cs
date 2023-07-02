using HousingProject.Core.Models.mpesaauthvm;
using HousingProject.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.CRUDServices.MainPaymentServices
{
   public   interface IpaymentServices
    {
        Task<mpesaAuthenticationvm> Getauthenticationtoken();
        Task<string> RegisterURL();
        Task<stk_push_response> STk_Push(string phoneNumber, decimal amount);
    }
}
