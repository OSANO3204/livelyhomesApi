using HousingProject.Architecture.IHouseRegistration_Services;
using HousingProject.Architecture.Interfaces.IlogginServices;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Houses.HouseAggrement;
using HousingProject.Core.ViewModel.Aggreement;
using HousingProject.Core.ViewModel.House;
using HousingProject.Core.ViewModel.House.HouseUsersvm;
using HousingProject.Core.ViewModel.HouseUnitRegistrationvm;
using HousingProject.Core.ViewModels;
using HousingProject.Infrastructure.Interfaces.IHouseRegistration_Services;
using HousingProject.Infrastructure.Response;
using HousingProject.Infrastructure.Response.BaseResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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

            return await _house_registrationservices.Registered_Houses();
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

            catch (Exception ex)
            {
                return new BaseResponse { Code = "145", ErrorMessage = ex.Message };
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

        [Authorize]
        [Route("Get_House_Users_Houses")]
        [HttpGet]
        public async Task<BaseResponse> Get_HouseUsers_Houses()
        {

            return await _house_registrationservices.Get_HouseUsers_Houses();
        }

        [Authorize]
        [Route("CreateAggreement")]
        [HttpPost]
        public Task<AggreementResponse> CreateAggreement(aggreementvm vm)
        {
            return _house_registrationservices.CreateAggreement(vm);
        }


        [Authorize]
        [Route("CreateAggeementSection")]
        [HttpPost]
        public Task AggreementSections(AggrementSections aggreementsection)
        {
            return _house_registrationservices.AggreementSections(aggreementsection);

        }

        [Authorize]
        [Route("NewSection")]
        [HttpPost]
        public async Task<AggreementResponse> AddSection(Sectionsvm vm)
        {
            return await _house_registrationservices.AddSection(vm);

        }


        [Authorize]
        [Route("GetAllSection")]
        [HttpGet]
        public async Task<AggreementResponse> GetAllAggreementSections()
        {

            return await _house_registrationservices.GetAllAggreementSections();
        }


        [Authorize]
        [Route("AddAggreementScetionToAggreement")]
        [HttpPost]
        public async Task<AggreementResponse> SelectAggeementSections(int aggreementID, int aggreeementSectionID)
        {


            return await _house_registrationservices.SelectAggeementSections(aggreementID, aggreeementSectionID);
        }



        [Authorize]
        [Route("GetAggrementsection")]
        [HttpPost]
        public async Task<classicaggreementresponse> GetAggementSections(int aggreemeniD)
        {

            return await _house_registrationservices.GetAggementSections(aggreemeniD);

        }

        [Authorize]
        [Route("GetAggrementsectionByHouseID")]
        [HttpPost]
        public async Task<classicaggreementresponse> GetAggementSectionsByHouseID(int HouseID)
        {

            return await _house_registrationservices.GetAggementSectionsByHouseID(HouseID);
        }

        [Authorize]
        [Route("GetAggeementbyHouseID")]
        [HttpPost]
        public async Task<AggreementResponse> GetggreementByHouseID(int houseid)
        {
            return await _house_registrationservices.GetggreementByHouseID(houseid);
        }

        [Authorize]
        [Route("GetHouseobjectById")]
        [HttpPost]
        public async Task<object> GethouseById(int houseid)
        {

            return await _house_registrationservices.GethouseById(houseid);
        }

        [Authorize]
        [Route("GetAggreementByTenantId")]
        [HttpPost]
        public async Task<BaseResponse> GetAggreementByTenantId(int tenantid)
        {

            return await _house_registrationservices.GetAggreementByTenantId(tenantid);
        }


        [Authorize]
        [Route("Get_Unoccupied_house_uints")]
        [HttpPost]

        public async Task<BaseResponse> GetUnoccupiedhouseunits(string housename)
        {

            return await _house_registrationservices.GetUnoccupiedhouseunits(housename);
        }

        [Authorize]
        [Route("Get_House_Profile")]
        [HttpPost]
        public async Task<Housing_Profile_Response> Get_House_Details_By_Id(int house_id)
        {
            return await _house_registrationservices.Get_House_Details_By_Id(house_id);

        }


        [Authorize]
        [Route("Update_House_unit_Status")]
        [HttpPost]
        public async Task<BaseResponse> Change_House_unit_Status(string house_name, int door_number, string unit_status)
        {
            return await _house_registrationservices.Change_House_unit_Status(house_name, door_number, unit_status);

        }

        [Authorize]
        [Route("Getting_all_houses")]
        [HttpGet]
        public async Task<BaseResponse> Getting_AllHouses()
        {
            return await _house_registrationservices.Getting_AllHouses();
        }


    }
}
