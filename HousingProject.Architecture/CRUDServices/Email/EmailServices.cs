using HousingProject.Architecture.Interfaces.IEmail;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Email;
using HousingProject.Core.ViewModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment _env;
        private readonly ILogger<IEmailServices>  _logger;
        //private readonly IConfiguration configuration;
        private readonly EmailConfiguration _emailconfig;

        public EmailServices(IOptions<EmailConfiguration> emailconfig, IHostingEnvironment env,
            ILogger<IEmailServices> logger)
        {
            _emailconfig = emailconfig.Value;
            _env = env;
            _logger = logger;
        }

        public async Task SendEmail(string mailText, string subject, string recipient)
        {
            try
            {
                var email = new MimeMessage { Sender = MailboxAddress.Parse(_emailconfig.SmtpUser) };
                var builder = new BodyBuilder { HtmlBody = mailText };
                email.Body = builder.ToMessageBody();
                email.To.Add(MailboxAddress.Parse(recipient));
                email.Subject= subject;
                using var smtp = new SmtpClient();
                await  smtp.ConnectAsync(_emailconfig.SmtpHost, Convert.ToInt32(_emailconfig.SmtpPort), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailconfig.EmailFrom, _emailconfig.SmtpPass);
                _logger.LogInformation("_____________________ 3 email sender links ________________________________");      
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
               
            }
            catch (Exception ex)
            {
                 _logger.LogInformation( "email sending error ___________+++++++++++++++___" +ex.Message );
            }
        }

        public async Task SentdirectlytonewTenant(UserEmailOptions options)
        {

            var templatesFolder = Path.Combine(_env.WebRootPath, "Templates\\Email\\");
            var templatePath = Path.Combine(templatesFolder, "emailsentToTenantonRegistration.html");
            var templateContent = System.IO.File.ReadAllText(templatePath);
            //var file = @"Templates/Email/emailsentToTenantonRegistration.html";
            //StreamReader str = new StreamReader(file);
            //string MailText = await str.ReadToEndAsync();
            //str.Close();
            templateContent = templateContent.Replace("User", options.UserName).Replace("user", options.PayLoad);
             await SendEmail(templateContent, "Dear Sir/Madam", options.ToEmail);

        }

        public async Task sendEmailOnHouseRegistration(UserEmailOptions options)
        {
            var templatesFolder = Path.Combine(_env.WebRootPath, "Templates\\Email\\");
            var templatePath = Path.Combine(templatesFolder, "EmailBody.html");
            var templateContent = System.IO.File.ReadAllText(templatePath);

            //var file = @"Templates/Email/EmailBody.html";
            //StreamReader str = new StreamReader(file);
            //string MailText = await str.ReadToEndAsync();
            //str.Close();
            templateContent = templateContent.Replace("User", options.UserName).Replace("user", options.PayLoad);
            await SendEmail(templateContent, "Dear Sir/Madam", options.ToEmail);

          

        }


        //regisered user
        public async Task EmailOnNewUserRegistrations(UserEmailOptions options)
        {
            var templatesFolder = Path.Combine(_env.WebRootPath, "Templates\\Email\\");
            var templatePath = Path.Combine(templatesFolder, "newuserRegistration.html");
            var templateContent = System.IO.File.ReadAllText(templatePath);
            //var file = @"Templates/Email/newuserRegistration.html";
            //StreamReader str = new StreamReader(file);
            //string MailText = await str.ReadToEndAsync();
            //str.Close();
            templateContent = templateContent.Replace("verificationstring", options.UserName).Replace("user", options.PayLoad);
            await SendEmail(templateContent, "Dear Sir/Madam", options.ToEmail);
           
         
           
        }

        public async Task newtenantemail(UserEmailOptions options)
        {

            var templatesFolder = Path.Combine(_env.WebRootPath, "Templates\\Email\\");
            var templatePath = Path.Combine(templatesFolder, "EmailOnTenantRegistration.html");
            var templateContent = System.IO.File.ReadAllText(templatePath);
            // var file = @"Templates/Email/EmailOnTenantRegistration.html";
            //StreamReader str = new StreamReader(file);
            //string MailText = await str.ReadToEndAsync();
            //str.Close();
            templateContent = templateContent.Replace("User", options.UserName).Replace("user", options.PayLoad);
             await SendEmail(templateContent, "Dear Sir/Madam", options.ToEmail);
           
               
          
        }

        public async Task  OnContusMessageSubmission(UserEmailOptions options)
        {
            var templatesFolder = Path.Combine(_env.WebRootPath, "Templates\\Email\\");
            var templatePath = Path.Combine(templatesFolder, "SentonContactUsmessage.html");
            var templateContent = System.IO.File.ReadAllText(templatePath);
           // var file = @"Templates/Email/SentonContactUsmessage.html";
            //StreamReader str = new StreamReader(file);
            //string MailText = await str.ReadToEndAsync();
            //str.Close();
            templateContent = templateContent.Replace("User", options.UserName).Replace("user", options.PayLoad);
             await SendEmail(templateContent, "Dear Sir/Madam", options.ToEmail);

           
        }

        //automatic emails
        public async Task EmailingAutomatically(UserEmailOptions options)
        {
            var templatesFolder = Path.Combine(_env.WebRootPath, "Templates\\Email\\");
            var templatePath = Path.Combine(templatesFolder, "automaticallysentbody.html");
            var templateContent = System.IO.File.ReadAllText(templatePath);
            //var file = @"Templates/Email/automaticallysentbody.html";
            //StreamReader str = new StreamReader(file);
            //string MailText = await str.ReadToEndAsync();
            //str.Close();
            templateContent = templateContent.Replace("User", options.UserName).Replace("user", options.PayLoad);
            await SendEmail(templateContent, "Dear Sir/Madam", options.ToEmail);
          
        }

        public async Task SendTenantEmailReminderOnRentPayment(TenantReminderEmail options)
        {
            var templatesFolder = Path.Combine(_env.WebRootPath, "Templates\\Email\\");
            var templatePath = Path.Combine(templatesFolder, "email_sent_to_remind_Tenant_of_rent_payment.html");
            var templateContent = System.IO.File.ReadAllText(templatePath);
            //var file = @"Templates/Email/email_sent_to_remind_Tenant_of_rent_payment.html";
            //StreamReader str = new StreamReader(file);
            //string MailText = await str.ReadToEndAsync();
            //str.Close();
            templateContent = templateContent.Replace("User", options.UserName).Replace("Body", options.PayLoad).Replace("Message", options.Message);
             await SendEmail(templateContent, "Dear Sir/Madam", options.ToEmail);

          

        }

        public async Task AutomatedNotificationonRentpayday(EmailNotificationOnRentPayment options)
        {
            var templatesFolder = Path.Combine(_env.WebRootPath, "Templates\\Email\\");
            var templatePath = Path.Combine(templatesFolder, "email_sent_to_remind_Tenant_of_rent_payment.html");
            var templateContent = System.IO.File.ReadAllText(templatePath);
            //var file = @"Templates/Email/email_sent_to_remind_Tenant_of_rent_payment.html";
            //StreamReader str = new StreamReader(file);
            //string MailText = await str.ReadToEndAsync();
            //str.Close();
            templateContent = templateContent.Replace("User", options.UserName).Replace("Body", options.PayLoad).Replace("Message", options.Message).Replace("sentDate", Convert.ToString(options.sentDate)).Replace("Names", options.UserName);
            await SendEmail(templateContent, "Dear Sir/Madam", options.ToEmail);
        }

         public async Task SendMessageReply(message_replybody options)
        {
            //var templatesFolder = Path.Combine(_env.WebRootPath, "Templates\\Email\\");
            //var templatePath = Path.Combine(templatesFolder, "replyMessage.html");
            //var templateContent = System.IO.File.ReadAllText(templatePath);
            var file = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "Templates" + Path.DirectorySeparatorChar.ToString()
               + "Email" + Path.DirectorySeparatorChar.ToString() + "replyMessage.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            file = file.Replace("receivername", options.Receivername).Replace("subject", options.Subject)
                .Replace("Message", options.Message)
                .Replace("sentDate", Convert.ToString(options.SentOn))
                .Replace("replymessage", options.replymessage)
                .Replace("agentname", options.AgentName)
                .Replace("companyphone", options.CompanyPhone);
            await SendEmail(file,  $"Dear {options.Receivername}", options.sendermail);
        }

        public async Task notificationOnRentPaymeentDay(AutomaticMessaging options)
        {
            var templatesFolder = Path.Combine(_env.WebRootPath, "Templates/Email/");
            var templatePath = Path.Combine(templatesFolder, "automated_rent_payment_Date.html");
            var templateContent = System.IO.File.ReadAllText(templatePath);
            //var file = @"Templates/Email/automated_rent_payment_Date.html";
            //StreamReader str = new StreamReader(file);
            //string MailText = await str.ReadToEndAsync();
            //str.Close();
            templateContent = templateContent
                 .Replace("User", options.TenantNmes)
                 .Replace("SentDate", options.SentDate.ToString())
                 .Replace("Message", options.Meessage)
                 .Replace("Names", options.ToEmail);
            await SendEmail(templateContent, "Dear Sir/Madam", options.ToEmail);
           
        }

        public async Task EmailOnSuccessfulLogin(UserEmailOptions emailbody)
        {

            var file = _env.WebRootPath+ Path.DirectorySeparatorChar.ToString()+ "Templates"+ Path.DirectorySeparatorChar.ToString()
                + "Email"+ Path.DirectorySeparatorChar.ToString()+ "emailOnSuccessfulLogin.html";
            //var file = @"Templates/Email/emailOnSuccessfulLogin.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            var currentyear = DateTime.Now.Year;
            MailText = MailText
                 .Replace("body", emailbody.PayLoad)
                 .Replace("names", emailbody.UserName)
                 .Replace("sentOn", Convert.ToString(DateTime.Now))
                 .Replace("currentyear", Convert.ToString(currentyear));
            await SendEmail(MailText, "Dear Sir/Madam", emailbody.ToEmail);
           
        }



        public async Task Email_successfull_payment(Payment_receipt_Email_Body emailbody)
        {
            var file =_env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "Templates" + Path.DirectorySeparatorChar.ToString()
                + "Email" + Path.DirectorySeparatorChar.ToString() + "receipt_On_Payment.html";
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
                .Replace("Balance", Convert.ToString(emailbody.Balance))
                .Replace("house_name", emailbody.HouseName)
                .Replace("DoorNumber", Convert.ToString(emailbody.DoorNumber))
                .Replace("House_Location", emailbody.HouseLocation)
                .Replace("caretaker_contact", emailbody.Caretaker_Phone)
                .Replace("currentyear", Convert.ToString(currentyear))
                .Replace("rent_amount", Convert.ToString(emailbody.Rent_Amount));

             await SendEmail(MailText, "Dear Sir/Madam", emailbody.ToEmail);
           
        }

        public async Task mail_To_Technician_On_Request(email_to_technician emailbody)
        {
            var file = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "Templates" + Path.DirectorySeparatorChar.ToString()
                + "Email" + Path.DirectorySeparatorChar.ToString() + "email_on_request_to_technician.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            var currentyear = DateTime.Now.Year;
            var currentDate = DateTime.Now;
            MailText = MailText
                //.Replace("body", emailbody.PayLoad)
                .Replace("user_name", emailbody.TechnicianNames)
                .Replace("names", emailbody.TechnicianNames)
                .Replace("Payload", "A  new repair request  that requires your attention has  been made, kindly log in to  Lively home to attend to it")
                .Replace("currentdate", Convert.ToString(currentDate));             
            await SendEmail(MailText, "Dear Sir/Madam", emailbody.ToEmail);
        }

        public async Task mail_To_Requester_On_Request(email_to_technician emailbody)
        {
            var file = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "Templates" + Path.DirectorySeparatorChar.ToString()
                + "Email" + Path.DirectorySeparatorChar.ToString() + "email_to_requester.html";
            StreamReader str = new StreamReader(file);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            var currentyear = DateTime.Now.Year;
            var currentDate = DateTime.Now;
            MailText = MailText
                //.Replace("body", emailbody.PayLoad)
                .Replace("user_name", emailbody.TechnicianNames)
                .Replace("names", emailbody.TechnicianNames)               
                .Replace("currentdate", Convert.ToString(currentDate));
            await SendEmail(MailText, "Dear Sir/Madam", emailbody.ToEmail);
        }

     

        //public async Task<BaseResponse> Tenant_Payment_History(string tenant_phone, string tenant_mail, string startdate, stringend )
    }
}
