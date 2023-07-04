using HousingProject.Architecture.Data;
using HousingProject.Architecture.Interfaces.IEmail;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Reply;
using HousingProject.Core.ViewModel;
using HousingProject.Core.ViewModel.Resplyvm;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
using HousingProject.Infrastructure.Interfaces.IUserExtraServices;
using HousingProject.Infrastructure.Response.ReplyResponse;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HousingProject.Infrastructure.CRUDServices.UsersExtra
{
     public   class UserExtraServices: IUserExtraServices
    {

        private readonly HousingProjectContext _context;
        private readonly IServiceScopeFactory _servicefactory;
        private object _loggedinuser;
        private readonly ILoggedIn _loggedinuserservices;

        public readonly IEmailServices _emailServices;

        public UserExtraServices
            (
            HousingProjectContext context,
            IServiceScopeFactory servicefactory,
            ILoggedIn loggedinuserservices,
            IEmailServices emailServices
            )
                {

                 _context = context;
                 _servicefactory = servicefactory;
                 _loggedinuserservices = loggedinuserservices;
            _emailServices = emailServices;
        }



        public async Task<BaseResponse> GetAllMessages()
        {
            using (var scope = _servicefactory.CreateScope())
            {
                try
                {
                   var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var allmessages = scopedcontext.ContactUs.Where(m => m.ClosedMessages == false).OrderByDescending(d=>d.DateCreated).ToList();
                    if (allmessages == null)
                    {
                        return new BaseResponse { Code = "140", ErrorMessage = "No messages found " };
                    }
                    var totalmessages = allmessages.Count();
                    return new BaseResponse { Code = "200", SuccessMessage = "Queried successfully", Body = allmessages ,Totals= totalmessages, isTrue=true};
                }
                catch(Exception ex)
                {
                    return new BaseResponse { Code = "120", ErrorMessage = ex.Message };
                }

            }
        }

        public async Task<BaseResponse> GeetMessageById(int messageid)
        {
            try
            {
               using (var scope = _servicefactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var messageexists =  scopedcontext.ContactUs.Where(m => m.ContacusId == messageid).FirstOrDefault();
                    if (messageexists == null)
                    {
                        return new BaseResponse { Code = "160", ErrorMessage = "message does not exist" };
                    }
                    return new BaseResponse { Code = "200", SuccessMessage = "Queried successfully", Body = messageexists };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "180", ErrorMessage = ex.Message };
            }
        }

        public async Task<messagereplyresponse> Replymessage(replyvm vm)
        {
            try
            {
                var loggedinuser = _loggedinuserservices.LoggedInUser().Result;
                using (var scope = _servicefactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var messagereply = new replyModel
                    {
                        MessageID = vm.MessageID,
                        ResponseAgent = loggedinuser.Email,
                        Reply = vm.Reply,
                        Closed=true
                    };
                    await scopedcontext.AddAsync(messagereply);
                    await scopedcontext.SaveChangesAsync();
                    var specificmessage =  scopedcontext.ContactUs.Where(m => m.ContacusId == vm.MessageID).FirstOrDefault();
                    if (specificmessage == null)
                    {
                        return new messagereplyresponse("140", "message not found", null);
                    }
                    specificmessage.ClosedMessages = true;
                    var userexists =  scopedcontext.RegistrationModel.Where(u => u.Email == vm.SenderMail).FirstOrDefault();
                    var emailbody = new message_replybody
                    {
                        Subject = specificmessage.Message_title,
                        Message = specificmessage.UserMessage,
                        AgentName = loggedinuser.FirstName +" "+ loggedinuser.LasstName,
                        CompanyPhone = "254721233204",
                        sendermail = specificmessage.Useremail,
                        Receivername=userexists.FirstName +"  " + userexists.LasstName,
                        replymessage=vm.Reply,
                        SentOn = DateTime.Now
                    };
                    var emailsent =  _emailServices.SendMessageReply(emailbody);
                    scopedcontext.Update(specificmessage);
                    await scopedcontext.SaveChangesAsync();
                    return new messagereplyresponse("200", "Reply sent successfully", messagereply);  
                }
            }
            catch (Exception ex)
            {
                return new messagereplyresponse("140", ex.Message, null);
            }
        }

        public async Task<messagereplyresponse> GetreplybymessageID(int messageid)
        {
            try
            {
                var loggedinuser = _loggedinuserservices.LoggedInUser().Result;
                using (var scope = _servicefactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var replieseists =  scopedcontext.replyModel.Where(r => r.MessageID == messageid).ToList();
                    if (replieseists == null)
                    {
                        return new messagereplyresponse("190", "message reply does npt exist", null);
                    }
                    return new messagereplyresponse("200", "Queried successfully", replieseists);
                }
            }
            catch (Exception ex)
            {
                return new messagereplyresponse("140", ex.Message, null);
            }
        }

        public async Task<BaseResponse> GetClosedMessages()
        {
            var allclosedmessages =  _context.ContactUs.Where(c => c.ClosedMessages == true).ToList();
            if (allclosedmessages == null)
            {
                return new BaseResponse {Code = "140", ErrorMessage = "No messages found " };
            };
             var totalmessages = allclosedmessages.Count();
            return new BaseResponse { Code = "200", SuccessMessage = "Queried successfully", Body = allclosedmessages, Totals = totalmessages, isTrue = true };
        }


        public string GenerateReferenceNumber(int length)
        {
            string ValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnop";
           
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(ValidChars.Length);
                sb.Append(ValidChars[randomIndex]);
            }
            return sb.ToString();
        }
    }


}
