using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.House;
using HousingProject.Core.ViewModel.House.HouseUsersvm;
using HousingProject.Core.ViewModels;
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

        //Task<string> uploadimage(IFormFile objfiles);

        //Task<AddImages> Create(IFormFile file);
    }
}
