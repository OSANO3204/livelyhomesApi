using HousingProject.Architecture.Response.Base;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions
{
    public  interface IverificationGenerator
    {
        Task<BaseResponse> GenerateToken();
    }
}
