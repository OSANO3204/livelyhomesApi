using HousingProject.Architecture.IPeopleManagementServvices;
using HousingProject.Architecture.Models.People;
using HousingProject.Architecture.Response.Base;
using HousingProject.Architecture.ViewModel.People;
using HousingProject.Core.ViewModel.People.GeneralRegistration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousingProject.API.Controllers.Registration
{
    [Route("api/[controller]", Name = "Entry_Point")]
    [ApiController] 

  
   

    public class RegistrationController:IRegistrationServices
    {
      
        public readonly IRegistrationServices _registerServices;
        public RegistrationController(IRegistrationServices registerServices)
        {
            _registerServices = registerServices;
        }

        [Authorize]
        [Route("GetRegisteredUsers")]
        [HttpGet]
        public async Task<IEnumerable> GetAllUsers()
        {
            
           return await _registerServices.GetAllUsers();
     
        }
        [Authorize]
        [Route("GetUserByUsername")]
        [HttpGet]
        public async Task<BaseResponse> GetUserByUsername(string username)
        {
           
                return await _registerServices.GetUserByUsername(username);

        }


        [Route("VerifyAccount")]
        [HttpPost]
        public async Task<BaseResponse> AccountVerification(string verificationtoken)
        {


            return await _registerServices.AccountVerification(verificationtoken);
        }


        [Route("RegisterNewUser")]
        [HttpPost]
        public async Task<BaseResponse> UserRegistration(RegisterViewModel registervm)
        {

            return await  _registerServices.UserRegistration(registervm);


        }

        [Route("asignrole")]
        [HttpPost]
        public async Task<BaseResponse> AsigRole(AsignRoleviewModel vm)
        {

          return  await _registerServices.AsigRole(vm);
        }


        [Route("Removerole")]
        [HttpPost]
        public async Task<BaseResponse> RomveRole(AsignRoleviewModel vm)
        {

            return await _registerServices.RomveRole(vm);
        }



    }
}
