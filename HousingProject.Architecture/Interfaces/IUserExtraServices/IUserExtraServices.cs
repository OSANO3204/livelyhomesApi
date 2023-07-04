using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.Resplyvm;
using HousingProject.Infrastructure.Response.ReplyResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Interfaces.IUserExtraServices
{
   public  interface IUserExtraServices
    {
        Task<BaseResponse> GetAllMessages();
        Task<BaseResponse> GeetMessageById(int messageid);
        Task<messagereplyresponse> Replymessage(replyvm vm);
        Task<messagereplyresponse> GetreplybymessageID(int messageid);
        Task<BaseResponse> GetClosedMessages();
        string GenerateReferenceNumber(int length);
    }
}
