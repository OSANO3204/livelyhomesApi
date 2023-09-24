using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel;
using HousingProject.Core.ViewModel.Professionalsvm;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
using HousingProject.Infrastructure.Interfaces.IProfessionalsServices;
using HousingProject.Infrastructure.Response;
using HousingProject.Infrastructure.Response.VotesResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace HousingProject.API.Controllers
{

    [Route("api/[controller]", Name = "Proessionals")]
    [ApiController]
    public class ProfessionalsController : ControllerBase
    {
        private readonly ILoggedIn _loggedIn;
        private readonly IProfessionalsServices _professionalsServices;
        public ProfessionalsController(IProfessionalsServices professionalsServices, ILoggedIn loggedIn)
        {
            _professionalsServices = professionalsServices;
            _loggedIn=loggedIn;

        }


        [Authorize]
        [HttpPost]
        [Route("Registerprojessional")]

        public async Task<BaseResponse> Createprofessonal(Professionalsvm vm)
        {

            return await _professionalsServices.Createprofessonal(vm);
        }


        [Authorize]
        [HttpPost]
        [Route("GetTechniciansByUsername")]
        public async Task<BaseResponse> GetTechnicianByName(string ProfesionName)
        {

            return await _professionalsServices.GetTechnicianByName(ProfesionName);
        }


        [Authorize]
        [HttpPost]
        [Route("GetTechniciansById")]
        public async Task<BaseResponse> GetProfessionalById(int id)
        {

            return await _professionalsServices.GetProfessionalById(id);
        }



        [Authorize]
        [HttpGet]
        [Route("GetTechnicianEmail")]
        public async Task<BaseResponse> GetProfessionalByEmail()
        {

            return await _professionalsServices.GetProfessionalByEmail();
        }


        [Authorize]
        [HttpPost]
        [Route("Update_Upvotes")]
        public async Task<VotesResponse> Update_UpVotes(int userid)
        {
            return await _professionalsServices.Update_UpVotes(userid);
        }

        [Authorize]
        [HttpPost]
        [Route("Update_Downvotes")]
        public async Task<VotesResponse> Update_DownVotes(int userid)
        {
            return await _professionalsServices.Update_DownVotes(userid);
        }

        [Authorize]
        [HttpPost]
        [Route("Get_User_rating")]
        public async Task<VotesResponse> Userrating(int userid)
        {
            return await _professionalsServices.Userrating(userid);

        }


        [Authorize]
        [HttpPost]
        [Route("Get_All_User_Services")]
        public async Task<BaseResponse> Get_User_Profession(string user_id)
        {
            var userid = user_id;

            var loged_in_user = _loggedIn.LoggedInUser().Result;

            return await _professionalsServices.Get_User_Profession(Convert.ToString(loged_in_user.Id));
        }

        [Authorize]
        [HttpPost]
        [Route("Get_tech+profle_with_job_number")]
        public async Task<professional_profile_Response> Get_technician_profile_with_job(string job_number)
        {
            return await _professionalsServices.Get_technician_profile_with_job(job_number);
        }


        [Authorize]
        [HttpPost]
        [Route("Get_techician_requests")]

        public async Task<BaseResponse> Get_Technician_Requests(string worker_email)
        {

            return await _professionalsServices.Get_Technician_Requests(worker_email);
        }

        [Authorize]
        [HttpPost]
        [Route("Add_techician_requests")]
        public async Task<BaseResponse> AddingRequest_to_Worker(add_request__vm vm)
        {

            return await _professionalsServices.AddingRequest_to_Worker(vm);
        }

        [Authorize]
        [HttpPost]
        [Route("Get_technician_with_jb_id")]
        public async Task<BaseResponse> Get_request_by_Job_Number(string job_number)
        {

            return await _professionalsServices.Get_request_by_Job_Number(job_number);
        }

        [Authorize]
        [HttpPost]
        [Route("Close_user_request")]
        public async Task<BaseResponse> Close_Request(int request_id)
        {
            return await _professionalsServices.Close_Request(request_id);
        }

        [Authorize]
        [HttpPost]
        [Route("Add_Services")]
        public async Task<BaseResponse> Add_Services(string service_added, string job_number)
        {
            return await _professionalsServices.Add_Services(service_added, job_number);
        }

        [Authorize]
        [HttpPost]
        [Route("Get_All_Services")]
        public async Task<BaseResponse> Get_Services_By_Job_Number(string job_number)
        {
            return await _professionalsServices.Get_Services_By_Job_Number(job_number);
        }



        [Authorize]
        [HttpPost]
        [Route("Get_All_my_Requests")]
        public async Task<BaseResponse> My_Repair_Requests()
        {
            return await _professionalsServices.My_Repair_Requests();
        }





    }
}
