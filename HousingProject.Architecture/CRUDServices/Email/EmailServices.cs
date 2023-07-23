using HousingProject.Architecture.Interfaces.IEmail;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Email;
using HousingProject.Core.ViewModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HousingProject.Architecture.CRUDServices.Email
{
    public class EmailServices : IEmailServices
    {
        private readonly ILogger<IEmailServices>  _logger;
        //private readonly IConfiguration configuration;
        private readonly EmailConfiguration _emailconfig;

        public EmailServices(IOptions<EmailConfiguration> emailconfig,
            ILogger<IEmailServices> logger)
        {
            _emailconfig = emailconfig.Value;
            _logger = logger;

        }

        public async Task<BaseResponse> SendEmail(string mailText, string subject, string recipient)
        {
            try
            {
                var email = new MimeMessage { Sender = MailboxAddress.Parse(_emailconfig.SmtpUser) };
                var builder = new BodyBuilder { HtmlBody = mailText };
                email.Body = builder.ToMessageBody();
                email.To.Add(MailboxAddress.Parse(recipient));
                email.Subject= subject;
                using var smtp = new SmtpClient();
                await  smtp.ConnectAsync(_emailconfig.SmtpHost, Convert.ToInt32(_emailconfig.SmtpPort), MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailconfig.EmailFrom, _emailconfig.SmtpPass);
                _logger.LogInformation("_____________________ 3 email sender links ________________________________");      
                var resped= await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                _logger.LogInformation("logging reponse : ", resped);
                return new BaseResponse { Code = "200", SuccessMessage = "Email sent successfully" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "001", ErrorMessage = ex.Message };
            }
        }

        public async Task<BaseResponse> SentdirectlytonewTenant(UserEmailOptions options)
        {
            var file = @"Templates/Email/emailsentToTenantonRegistration.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            MailText = MailText.Replace("User", options.UserName).Replace("user", options.PayLoad);
            var result = await SendEmail(MailText, "Dear Sir/Madam", options.ToEmail);

            if (result.Code == "200")
            {

                return new BaseResponse { Code = "200", ErrorMessage = "Tenant successfully and email sent to them" };
            }
            return new BaseResponse { Code = "350", ErrorMessage = "Failed to send email" };
        }

        public async Task<BaseResponse> sendEmailOnHouseRegistration(UserEmailOptions options)
        {

            var file = @"Templates/Email/EmailBody.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            MailText = MailText.Replace("User", options.UserName).Replace("user", options.PayLoad);
            var result = await SendEmail(MailText, "Dear Sir/Madam", options.ToEmail);

            if (result.Code == "200")
            {

                return new BaseResponse { Code = "200", ErrorMessage = "House registered successfully and email sent" };
            }
            return new BaseResponse { Code = "350", ErrorMessage = "Failed to send email" };
        }


        //regisered user
        public async Task<BaseResponse> EmailOnNewUserRegistrations(UserEmailOptions options)
        {
            var file = @"Templates/Email/newuserRegistration.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            MailText = MailText.Replace("verificationstring", options.UserName).Replace("user", options.PayLoad);
            var result = await SendEmail(MailText, "Dear Sir/Madam", options.ToEmail);

            if (result.Code == "200")
            {

                return new BaseResponse { Code = "200"};
            }
            return new BaseResponse { Code = "350" };
        }

        public async Task<BaseResponse> newtenantemail(UserEmailOptions options)
        {
            var file = @"Templates/Email/EmailOnTenantRegistration.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            MailText = MailText.Replace("User", options.UserName).Replace("user", options.PayLoad);
            var result = await SendEmail(MailText, "Dear Sir/Madam", options.ToEmail);

            if (result.Code == "200")
            {

                return new BaseResponse { Code = "200" };
            }
            return new BaseResponse { Code = "350" };
        }

        public async Task<BaseResponse> OnContusMessageSubmission(UserEmailOptions options)
        {
            var file = @"Templates/Email/SentonContactUsmessage.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            MailText = MailText.Replace("User", options.UserName).Replace("user", options.PayLoad);
            var result = await SendEmail(MailText, "Dear Sir/Madam", options.ToEmail);

            if (result.Code == "200")
            {

                return new BaseResponse { Code = "200" };
            }
            return new BaseResponse { Code = "350" };


        }

        //automatic emails
        public async Task<BaseResponse> EmailingAutomatically(UserEmailOptions options)
        {
            var file = @"Templates/Email/automaticallysentbody.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            MailText = MailText.Replace("User", options.UserName).Replace("user", options.PayLoad);
            var result = await SendEmail(MailText, "Dear Sir/Madam", options.ToEmail);
            if (result.Code == "200")
            {
                return new BaseResponse { Code = "200" };
            }
            return new BaseResponse { Code = "350" };
        }

        public async Task<BaseResponse> SendTenantEmailReminderOnRentPayment(TenantReminderEmail options)
        {
            var file = @"Templates/Email/email_sent_to_remind_Tenant_of_rent_payment.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            MailText = MailText.Replace("User", options.UserName).Replace("Body", options.PayLoad).Replace("Message", options.Message);
            var result = await SendEmail(MailText, "Dear Sir/Madam", options.ToEmail);

            if (result.Code == "200")
            {
                return new BaseResponse { Code = "200" };
            }
            else
            {
                return new BaseResponse { Code = "350" };
            }

        }

        public async Task AutomatedNotificationonRentpayday(EmailNotificationOnRentPayment options)
        {
            var file = @"Templates/Email/email_sent_to_remind_Tenant_of_rent_payment.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            MailText = MailText.Replace("User", options.UserName).Replace("Body", options.PayLoad).Replace("Message", options.Message).Replace("sentDate", Convert.ToString(options.sentDate)).Replace("Names", options.UserName);
            await SendEmail(MailText, "Dear Sir/Madam", options.ToEmail);
        }

         public async Task SendMessageReply(message_replybody options)
        {
            var file = @"Templates/Email/replyMessage.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            MailText = MailText.Replace("receivername", options.Receivername).Replace("subject", options.Subject)
                .Replace("Message", options.Message)
                .Replace("sentDate", Convert.ToString(options.SentOn))
                .Replace("replymessage", options.replymessage)
                .Replace("agentname", options.AgentName)
                .Replace("companyphone", options.CompanyPhone);
            await SendEmail(MailText,  $"Dear {options.Receivername}", options.sendermail);
        }

        public async Task<BaseResponse> notificationOnRentPaymeentDay(AutomaticMessaging options)
        {
            var file = @"Templates/Email/automated_rent_payment_Date.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            MailText = MailText
                 .Replace("User", options.TenantNmes)
                 .Replace("SentDate", options.SentDate.ToString())
                 .Replace("Message", options.Meessage)
                 .Replace("Names", options.ToEmail);
            var response=    await SendEmail(MailText, "Dear Sir/Madam", options.ToEmail);
            if (response.Code == "200")
            {       
                _logger.LogInformation("sent mailsuccessfully");
                return new BaseResponse { Code = "200", SuccessMessage = "Email successfully sent" };
            }
            else
            {
               _logger.LogInformation(response.ErrorMessage);
                return new BaseResponse { Code = "250", SuccessMessage = "Email not sent" };
            }
        }

        public async Task<BaseResponse>  EmailOnSuccessfulLogin(UserEmailOptions  emailbody)
        {
            var file = @"Templates/Email/emailOnSuccessfulLogin.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            var currentyear = DateTime.Now.Year;
            MailText = MailText
                 .Replace("body", emailbody.PayLoad)
                 .Replace("names", emailbody.UserName)
                 .Replace("sentOn", Convert.ToString(DateTime.Now))
                 .Replace("currentyear",Convert.ToString(currentyear));
            var response = await SendEmail(MailText, "Dear Sir/Madam", emailbody.ToEmail);
            if (response.Code == "200")
            {
                _logger.LogInformation("sent mailsuccessfully");
                return new BaseResponse { Code = "200", SuccessMessage = "Email successfully sent" };
            }
            else
            {
                _logger.LogInformation(response.ErrorMessage);
                return new BaseResponse { Code = "250", SuccessMessage = "Email not sent" };
            }
        }

      
      
      
     


        public async Task<BaseResponse> Email_successfull_payment(Payment_receipt_Email_Body emailbody)
        {
            var file = @"Templates/Email/receipt_On_Payment.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            var currentyear = DateTime.Now.Year;
            MailText = MailText
                 //.Replace("body", emailbody.PayLoad)
                 .Replace("user_name", emailbody.UserName)
                 .Replace("sentOn", Convert.ToString(DateTime.Now))
                 .Replace("tenant_names", emailbody.TenantNames)
                  .Replace("tenant_phone", emailbody.Tenant_Phone)
                  .Replace("Balance",Convert.ToString(emailbody.Balance))
                  .Replace("house_name", emailbody.HouseName)
                  .Replace("DoorNumber", Convert.ToString(emailbody.DoorNumber))
                  .Replace("House_Location", emailbody.HouseLocation)
                  .Replace("caretaker_contact", emailbody.Caretaker_Phone)
                   .Replace("currentyear", Convert.ToString(currentyear))
                    .Replace("rent_amount", Convert.ToString(emailbody.Rent_Amount));

            var response = await SendEmail(MailText, "Dear Sir/Madam", emailbody.ToEmail);
            if (response.Code == "200")
            {
                _logger.LogInformation("sent mailsuccessfully");
                return new BaseResponse { Code = "200", SuccessMessage = "Email successfully sent" };
            }
            else
            {
                _logger.LogInformation(response.ErrorMessage);
               return new BaseResponse { Code = "120", SuccessMessage = "Email not sent" };
            }
        }
    }
}
