using HousingProject.Architecture.Response.Base;
using HousingProject.Infrastructure.Interfaces.IUserExtraServices;
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

    }
}
