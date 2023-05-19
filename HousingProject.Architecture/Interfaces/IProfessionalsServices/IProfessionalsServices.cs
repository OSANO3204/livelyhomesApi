using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.Professionalsvm;
using HousingProject.Infrastructure.Response.VotesResponse;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Interfaces.IProfessionalsServices
{
    public    interface IProfessionalsServices
    {
        Task<BaseResponse> Createprofessonal(Professionalsvm vm);
        Task<BaseResponse> GetTechnicianByName(string ProfesionName);

         Task<BaseResponse> GetProfessionalById(int id);

         Task<BaseResponse> GetProfessionalByEmail();
        Task<VotesResponse> Update_UpVotes(int userid);
        Task<VotesResponse> Update_DownVotes(int userid);
        Task<VotesResponse> Userrating(int userid);


    }
}
