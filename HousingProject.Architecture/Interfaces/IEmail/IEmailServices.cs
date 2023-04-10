using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Architecture.Interfaces.IEmail
{
    public interface IEmailServices
    {
        Task<BaseResponse> SendEmail(string mailText, string subject, string recipient);
        Task<BaseResponse> sendEmailOnHouseRegistration(UserEmailOptions options);
        Task<BaseResponse> EmailOnNewUserRegistrations(UserEmailOptions options);
        Task<BaseResponse> newtenantemail(UserEmailOptions options);
        Task<BaseResponse> OnContusMessageSubmission(UserEmailOptions options);
        Task<BaseResponse> SentdirectlytonewTenant(UserEmailOptions options);
        Task<BaseResponse> EmailingAutomatically(UserEmailOptions options);
        Task<BaseResponse> SendTenantEmailReminderOnRentPayment(TenantReminderEmail options);
    }
}
