using HousingProject.Architecture.Response.Base;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions.RolesDescription
{
    public  interface IRoles
    {
        Task<BaseResponse> CheckUserRole();
    }
}
