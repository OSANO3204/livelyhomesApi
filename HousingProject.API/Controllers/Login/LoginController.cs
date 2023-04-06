using HousingProject.Architecture.Interfaces.IlogginServices;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.People;
using HousingProject.Core.Models.People.General;
using HousingProject.Core.ViewModel.Passwords;
using HousingProject.Core.ViewModel.People.GeneralRegistration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HousingProject.API.Controllers.Login
{
    [Route("api/[controller]", Name = "Landlord")]
    [ApiController]
    public class LoginController : IloggedInServices
    {
        private readonly IloggedInServices _iloggedInServices;

        public LoginController(IloggedInServices iloggedInServices)
        {
            _iloggedInServices = iloggedInServices;
        }
        [Route("Authenticate")]
        [HttpPost]
        public async Task<authenticationResponses> Authenticate(UserLogin loggedinuser)
        {
            return await _iloggedInServices.Authenticate(loggedinuser);
        }

        [Authorize]
        [Route("LoggedInUser")]
        [HttpGet]
        public async   Task<RegistrationModel> LoggedInUser()
        {
            var res = await _iloggedInServices.LoggedInUser();
            return res;
        }

        [Authorize]
        [Route("getuserroles")]
        [HttpGet]

        public async Task<BaseResponse> GetUserroles()
        {

            return await _iloggedInServices.GetUserroles();
        }

        [Authorize]
        [Route("ContactUs")]
        [HttpPost]
        public async Task<BaseResponse> ContactUs(ContactUsViewModel vm)

        {
        return   await   _iloggedInServices.ContactUs(vm);
        }

        [Authorize]
        [Route("ChangeEmail")]
        [HttpPost]

        public async Task<BaseResponse> ChangeUserEmail(string emailaddress)
        {
            return  await _iloggedInServices.ChangeUserEmail(emailaddress);
        }

      
        [Route("Reset_Password")]
        [HttpPost]
        public async Task<BaseResponse> ResetPassword(ResetPassword resetpasswordvm)
        {
            return await _iloggedInServices.ResetPassword(resetpasswordvm);
        }

        [Authorize]
        [Route("ChangeFirstName")]
        [HttpPost]
        public async Task<BaseResponse> ChangeFirstName(string FirstName)
        {
            return await _iloggedInServices.ChangeFirstName(FirstName);
        }

        [Authorize]
        [Route("ChangeLasttName")]
        [HttpPost]
        public async Task<BaseResponse> ChangeLastName(string LastName)
        {
            return await _iloggedInServices.ChangeLastName(LastName);
        }

    }
}
