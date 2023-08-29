using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel;
using HousingProject.Core.ViewModel.Professionalsvm;
using HousingProject.Infrastructure.Response;
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
        Task<BaseResponse> Get_User_Profession(string user_id);
        Task<professional_profile_Response> Get_technician_profile_with_job(string job_number);
        Task<BaseResponse> AddingRequest_to_Worker(add_request__vm vm);
        Task<BaseResponse> Get_Technician_Requests(string worker_email);
        Task<BaseResponse> Get_request_by_Job_Number(string job_number);
        Task<BaseResponse> Close_Request(int request_id);
        Task<BaseResponse> Add_Services(string service_added, string job_number);
        Task<BaseResponse> Get_Services_By_Job_Number(string job_number);
        Task<BaseResponse> My_Repair_Requests();

    }
}
