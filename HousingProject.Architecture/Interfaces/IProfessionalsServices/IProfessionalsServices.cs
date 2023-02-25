using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.Professionalsvm;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Interfaces.IProfessionalsServices
{
    public    interface IProfessionalsServices
    {
        Task<BaseResponse> Createprofessonal(Professionalsvm vm);
        Task<BaseResponse> GetTechnicianByName(string ProfesionName);

         Task<BaseResponse> GetProfessionalById(int id);

         Task<BaseResponse> GetProfessionalByEmail();

       
    }
}
