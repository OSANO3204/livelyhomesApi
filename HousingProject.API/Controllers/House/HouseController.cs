using HousingProject.Architecture.IHouseRegistration_Services;
using HousingProject.Architecture.Interfaces.IlogginServices;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.House;
using HousingProject.Core.ViewModel.House.HouseUsersvm;
using HousingProject.Core.ViewModel.HouseUnitRegistrationvm;
using HousingProject.Core.ViewModels;
using HousingProject.Infrastructure.Interfaces.IHouseRegistration_Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HousingProject.API.Controllers.House
{


    [Route("api/[controller]", Name = "Building_Apartment")]
    [ApiController]

    public class HouseController : IHouse_RegistrationServices
    {

        public readonly IHouse_RegistrationServices _house_registrationservices;
        private readonly IHttpContextAccessor _httpcontextaccessor;
        private readonly IloggedInServices _iloggedInServices;
        private readonly IHouseUnits _houseUnits;


        public HouseController(IHouse_RegistrationServices house_registrationservices, IHttpContextAccessor httpcontextaccessor, IloggedInServices iloggedInServices, IHouseUnits houseUnits)
        {

            _house_registrationservices = house_registrationservices;
            _httpcontextaccessor = httpcontextaccessor;
            _iloggedInServices = iloggedInServices;
            _houseUnits = houseUnits;
        }

        [Authorize]
        [Route("Get_Registererd_House")]
        [HttpGet]
         public async Task<BaseResponse> Registered_Houses()
        {

            return await  _house_registrationservices.Registered_Houses();
        }


        [Authorize]
        [Route("Register_House")]
        [HttpPost]
        public async Task<BaseResponse> Register_House(HouseRegistrationViewModel newvm)
        {
            try
            {
                return await _house_registrationservices.Register_House(newvm);
            }

            catch(Exception ex ) 
            {
                return new BaseResponse { Code="145", ErrorMessage= ex.Message}; 
            }
           
        }

        [Authorize]
        [Route("GetHousesBy_Owner_Id")]
        [HttpGet]

        public async Task<BaseResponse> GetHousesBy_OwnerIdNumber(int OwnerId)
        {

            return await _house_registrationservices.GetHousesBy_OwnerIdNumber(OwnerId);
        }

        [Authorize]
        [Route("GetHouseByLocation")]
        [HttpGet]
        public async Task<BaseResponse> GetHoousesByLocation(string House_Location)
        {


            return await _house_registrationservices.GetHoousesByLocation(House_Location);
        }

        [Authorize]
        [Route("AddAdminContacts")]
        [HttpPost]
        public async Task<BaseResponse> AddAdminContacts(AdminContctsViewModel vm)
        {

            return await _house_registrationservices.AddAdminContacts(vm);
        }
        [Authorize]
        [Route("GetTotalHousesByUser")]
        [HttpPost]
        public async Task<BaseResponse> TotalHusesManaged(string email)
        {

            return await _house_registrationservices.TotalHusesManaged(email);
        }


        [Authorize]
        [Route("CreateHouseUser")]
        [HttpPost]
        public async Task<BaseResponse> CreateHouseUser(HouseUsersViewModel vm)
        {

            return await _house_registrationservices.CreateHouseUser(vm);
        }


        [Authorize]
        [Route("HouseUsers")]
        [HttpPost]
        public async Task<BaseResponse> GetHouseUser(int houseid)
        {


            return await _house_registrationservices.GetHouseUser(houseid);
        }

        [Authorize]
        [Route("GetHousenamebyid")]
        [HttpPost]
        public async Task<BaseResponse> gethouseById(int houseid)
        {

            return await _house_registrationservices.gethouseById(houseid);
        }

        [Authorize]
        [Route("RegisterHouseUnits")]
        [HttpPost]

        public async Task<BaseResponse> RegisterHouseUnit(HouseUnitRegistrationvm vm)
        {

            return await _houseUnits.RegisterHouseUnit(vm);
        }

    }
}
