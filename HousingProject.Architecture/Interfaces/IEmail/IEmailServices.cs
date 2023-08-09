using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Email;
using HousingProject.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Architecture.Interfaces.IEmail
{
    public interface IEmailServices
    {
        Task SendEmail(string mailText, string subject, string recipient);
        Task sendEmailOnHouseRegistration(UserEmailOptions options);
        Task EmailOnNewUserRegistrations(UserEmailOptions options);
        Task newtenantemail(UserEmailOptions options);
        Task OnContusMessageSubmission(UserEmailOptions options);
        Task SentdirectlytonewTenant(UserEmailOptions options);
        Task EmailingAutomatically(UserEmailOptions options);
        Task SendTenantEmailReminderOnRentPayment(TenantReminderEmail options);
        Task SendMessageReply(message_replybody options);
        Task notificationOnRentPaymeentDay(AutomaticMessaging options);
        Task EmailOnSuccessfulLogin(UserEmailOptions emailbody);
        Task Email_successfull_payment(Payment_receipt_Email_Body emailbody);
    }
}
