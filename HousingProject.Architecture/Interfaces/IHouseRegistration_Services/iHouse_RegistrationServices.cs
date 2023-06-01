using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Houses.HouseAggrement;
using HousingProject.Core.ViewModel.Aggreement;
using HousingProject.Core.ViewModel.House;
using HousingProject.Core.ViewModel.House.HouseUsersvm;
using HousingProject.Core.ViewModel.HouseUnitRegistrationvm;
using HousingProject.Core.ViewModels;
using HousingProject.Infrastructure.Response.BaseResponses;
using System.Threading.Tasks;

namespace HousingProject.Architecture.IHouseRegistration_Services
{
    public  interface IHouse_RegistrationServices
    {
        
        Task<BaseResponse> Register_House(HouseRegistrationViewModel newvm);
        Task<BaseResponse> Registered_Houses();
        Task<BaseResponse> GetHousesBy_OwnerIdNumber(int OwnerId);
        Task<BaseResponse> GetHoousesByLocation(string House_Location);
        Task<BaseResponse> AddAdminContacts(AdminContctsViewModel vm);
        Task<BaseResponse> TotalHusesManaged(string email);
        Task<BaseResponse> GetHouseUser(int houseid);
        Task<BaseResponse> CreateHouseUser(HouseUsersViewModel vm);
        Task<BaseResponse> gethouseById(int houseid);
        Task<BaseResponse> Get_HouseUsers_Houses();
        Task<AggreementResponse> CreateAggreement(aggreementvm vm);
        Task<AggreementResponse> AddSection(Sectionsvm vm);
        Task<AggreementResponse> GetAllAggreementSections();
        Task AggreementSections(AggrementSections aggreementsection);
        Task<AggreementResponse> SelectAggeementSections(int aggreementID, int aggreeementSectionID);
        Task<classicaggreementresponse> GetAggementSections(int aggreemeniD);
         Task<classicaggreementresponse> GetAggementSectionsByHouseID(int HouseID);

        Task<AggreementResponse> GetggreementByHouseID(int houseid);
        Task<object> GethouseById(int houseid);
        //Task<string> uploadimage(IFormFile objfiles);

        //Task<AddImages> Create(IFormFile file);
        Task<BaseResponse> GetAggreementByTenantId(int tenantid);
        Task<BaseResponse> GetUnoccupiedhouseunits(string housename);
    }
}
