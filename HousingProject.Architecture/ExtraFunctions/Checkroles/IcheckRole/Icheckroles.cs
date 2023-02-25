using HousingProject.Architecture.Response.Base;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions.Checkroles.IcheckRole
{
    public   interface ICheckroles
    {
        Task<BaseResponse> CheckIfTenant();
        Task<BaseResponse> CheckIfLandlord();
        Task<BaseResponse> CheckIfAgent();
        Task<BaseResponse> CheckIfCareTaker();

    }
}
