using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.Resplyvm;
using HousingProject.Infrastructure.Interfaces.IUserExtraServices;
using HousingProject.Infrastructure.Response.ReplyResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousingProject.API.Controllers.UserExtra
{

    [Route("api/[controller]", Name = "User_Extra")]
    [ApiController]

     public class UserExtraController : Controller
     {
        private readonly IUserExtraServices _userExtraServices;
        public UserExtraController(IUserExtraServices userExtraServices)
        {
            _userExtraServices = userExtraServices;
        }

        [HttpGet]
        [Route("GetAllMessages")]
        [Authorize]
        public async Task<BaseResponse> GetAllMessages()
        {
            return await  _userExtraServices.GetAllMessages();
        }

        [HttpPost]
        [Route("GetMessagesbyId")]
        [Authorize]
        public async Task<BaseResponse> GeetMessageById(int messageid)
        {
            return  await _userExtraServices.GeetMessageById(messageid);
        }

        [HttpPost]
        [Route("Replymessage")]
        [Authorize]
        public async Task<messagereplyresponse> Replymessage(replyvm vm)
        {
            return await _userExtraServices.Replymessage(vm);
        }

        [HttpPost]
        [Route("GetAllRepliesByMessageId")]
        [Authorize]
        public async Task<messagereplyresponse> GetreplybymessageID(int messageid)
        {

            return await  _userExtraServices.GetreplybymessageID(messageid);
        }

        [HttpGet]
        [Route("GetAll_closed_messages")]
        [Authorize]
        public async Task<BaseResponse> GetClosedMessages()
        {
            return await _userExtraServices.GetClosedMessages();

        }

    }
}
