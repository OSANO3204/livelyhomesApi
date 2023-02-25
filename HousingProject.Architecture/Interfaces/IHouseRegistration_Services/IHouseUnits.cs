using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.HouseUnitRegistrationvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Interfaces.IHouseRegistration_Services
{
   public  interface IHouseUnits
    {
        Task<BaseResponse> RegisterHouseUnit(HouseUnitRegistrationvm vm);
    }
}
