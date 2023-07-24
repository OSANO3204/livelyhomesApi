using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.mpesaauthvm;
using HousingProject.Infrastructure.Response;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.CRUDServices.MainPaymentServices
{
    public  interface IpaymentServices
    {
        Task<mpesaAuthenticationvm> Getauthenticationtoken();
        Task<string> RegisterURL();
        Task<stk_push_response> STk_Push(string phoneNumber, decimal amount);
        Task Get_CallBack_Body(JObject requestBody);
        Task<string> RegisterConfirmationUrl();
        Task<string> RegisterValidationUrl();
        Task SendReceipts();
        Task<BaseResponse> GetPaginatedTransactions(int pageNumber);
    }
}
