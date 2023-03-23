using HousingProject.Architecture.Interfaces.IEmail;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Email;
using HousingProject.Core.ViewModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HousingProject.Architecture.CRUDServices.Email
{
    public class EmailServices : IEmailServices
    {

        //private readonly IConfiguration configuration;
        private readonly EmailConfiguration _emailconfig;

        public EmailServices(IOptions<EmailConfiguration> emailconfig)
        {
            _emailconfig = emailconfig.Value;


        }


        public async Task<BaseResponse> SendEmail(string mailText, string subject, string recipient)
        {
            try
            {

                var email = new MimeMessage { Sender = MailboxAddress.Parse(_emailconfig.SmtpUser) };
                email.To.Add(MailboxAddress.Parse(recipient));
                email.Subject = subject;
                var builder = new BodyBuilder { HtmlBody = mailText };
                email.Body = builder.ToMessageBody();
                var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailconfig.SmtpHost, Convert.ToInt32(_emailconfig.SmtpPort), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailconfig.EmailFrom, _emailconfig.SmtpPass);
                var response = await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);

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
    }
}
