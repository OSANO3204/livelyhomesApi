using HousingProject.Architecture.Response.Base;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions.RolesDescription
{


   public class Roles: IRoles
    {
        private  readonly ILoggedIn _loggedin;
        public Roles(ILoggedIn loggedin)
        {
            _loggedin = loggedin;
        }



        public async Task<BaseResponse> CheckUserRole()
        {

            var user = await  _loggedin.LoggedInUser();
            try
            {
                if (user == null)
                {

                    return new BaseResponse { Code = "20", ErrorMessage = "User not found please log in again or reload your page" };
                }

                if (user.Is_Agent && user.Is_CareTaker && user.Is_Landlord)
                {

                    return new BaseResponse { SuccessMessage = "superadmin" };
                }
                if (user.Is_Agent && user.Is_CareTaker)
                {

                    return new BaseResponse { SuccessMessage = "Caretakeragent" };
                }

          


                if (user.Is_Agent && user.Is_Tenant)
                {

                    return new BaseResponse { SuccessMessage = "Agenttenant" };
                }

                if (user.Is_Landlord && user.Is_CareTaker)
                {

                    return new BaseResponse { SuccessMessage = "CaretakerLandlord" };
                }
                if (user.Is_Agent && user.Is_Landlord)
                {

                    return new BaseResponse { SuccessMessage = "Agentlandlord" };
                }

               

                if (user.Is_Agent && user.Is_CareTaker)
                {

                    return new BaseResponse { SuccessMessage = "Caretakergaent" };
                }


                if (user.Is_Agent)
                {


                    return new BaseResponse { SuccessMessage = "Agent" };


                }
                if (user.Is_CareTaker)
                {
                    return new BaseResponse { SuccessMessage = "Caretaker" };
                }

                if (user.Is_Tenant)
                {

                    return new BaseResponse { Code = "13", SuccessMessage = "Tenant" };
                }

                if (user.Is_Landlord)
                {
                    return new BaseResponse { Code = "13", SuccessMessage = "Landlord" };

                }
            }
            catch (Exception ex)
            {

                foreach (var error in ex.Message)
                {

                    return new BaseResponse { Code = "12", ErrorMessage =error.ToString() };
                }
            }return new BaseResponse { Code = "123", ErrorMessage = "There is just no helping you , you have no role" };

        }
    }
}
