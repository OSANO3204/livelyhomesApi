using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.People;
using HousingProject.Core.Models.People.General;
using HousingProject.Core.ViewModel.Passwords;
using HousingProject.Core.ViewModel.People.GeneralRegistration;
using System.Threading.Tasks;

namespace HousingProject.Architecture.Interfaces.IlogginServices
{
    public interface IloggedInServices
    {

        Task<BaseResponse> ResetPassword(ResetPassword resetpasswordvm);
        Task<authenticationResponses> Authenticate(UserLogin loggedinuser);
        Task<RegistrationModel> LoggedInUser();
        Task<BaseResponse> ChangeUserEmail(string emailaddress);
        Task<BaseResponse> ChangeFirstName(string FirstName);
        Task<BaseResponse> ChangeLastName(string LastName);
        Task<BaseResponse> ContactUs(ContactUsViewModel vm);
        Task<BaseResponse> GetUserroles();
    


        }
}
