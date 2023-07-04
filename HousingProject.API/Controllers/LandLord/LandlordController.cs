using HousingProject.Architecture.Interfaces.ILandlordModel;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.Landlord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;

namespace HousingProject.API.Controllers.LandLord
{

    [Authorize]
    [Route("api/[controller]", Name = "Landlord")]
    [ApiController]

    public class LandlordController : ILandlordServices
    {
        public readonly ILandlordServices _ilandlordservices;
        public LandlordController(ILandlordServices ilandlordservices)
        {
            _ilandlordservices = ilandlordservices;
        }

        [Authorize]
        [Route("GetAll_Landlord_Registered_Houses")]
        [HttpGet]
        public Task<IEnumerable> GetLandlordRegisteredHouses()
        {
            return _ilandlordservices.GetLandlordRegisteredHouses();
        }

        [Authorize]
        [Route("Landlord_House_Registration")]
        [HttpPost]
        public Task<BaseResponse> LandlongHouse_Registration(LandlordHouse_RegistrationVm vm)
        {
            return _ilandlordservices.LandlongHouse_Registration(vm);
        }


    }
}
