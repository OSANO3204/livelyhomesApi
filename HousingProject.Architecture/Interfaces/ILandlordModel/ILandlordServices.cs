using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.Landlord;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Architecture.Interfaces.ILandlordModel
{
   public  interface ILandlordServices
    {
       
        Task<BaseResponse> LandlongHouse_Registration(LandlordHouse_RegistrationVm vm);

        Task<IEnumerable> GetLandlordRegisteredHouses();

    }
}
