using HousingProject.Architecture.Response.Base;
using HousingProject.Infrastructure.ExtraFunctions.Checkroles.IcheckRole;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions.Checkroles.ChcekRoles
{
    public class CheckRoles : ICheckroles
    {

        private readonly ILoggedIn _loggedIn;
        public CheckRoles(ILoggedIn loggedIn)
        {
            _loggedIn = loggedIn;
        }

        public async Task<BaseResponse> CheckIfAgent()
        {
            var user = await _loggedIn.LoggedInUser();

            if (user.Is_Agent)
            {

                return new BaseResponse { isTrue = true };

            }

            else 
            {

                return new BaseResponse { isTrue = false };
            }
        }

        public async Task<BaseResponse> CheckIfCareTaker()
        {
            var user = await _loggedIn.LoggedInUser();

            if (user.Is_CareTaker)
            {

                return new BaseResponse { isTrue = true };

            }

            else
            {

                return new BaseResponse { isTrue = false };
            }
        }

        public async Task<BaseResponse> CheckIfLandlord()
        {
            var user = await _loggedIn.LoggedInUser();

            if (user.Is_Landlord)
            {

                return new BaseResponse { isTrue = true };

            }

            else
            {

                return new BaseResponse { isTrue = false };
            }
        }

        public async Task<BaseResponse> CheckIfTenant()
        {

            var user = await _loggedIn.LoggedInUser();

            if (user.Is_Tenant)
            {

                return new BaseResponse { isTrue = true };

            }

            else
            {

                return new BaseResponse { isTrue = false };

            }
        }
    }
}
