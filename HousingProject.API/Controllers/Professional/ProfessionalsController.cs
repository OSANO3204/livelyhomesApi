using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.Professionalsvm;
using HousingProject.Infrastructure.Interfaces.IProfessionalsServices;
using HousingProject.Infrastructure.Response.VotesResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace HousingProject.API.Controllers
{

    [Route("api/[controller]", Name = "Proessionals")]
    [ApiController]
    public class ProfessionalsController : ControllerBase
    {

        private readonly IProfessionalsServices _professionalsServices;
        public ProfessionalsController(IProfessionalsServices professionalsServices)
        {
            _professionalsServices = professionalsServices;

        }




        [Authorize]
        [HttpPost]
        [Route("Registerprojessional")]

       public async  Task<BaseResponse> Createprofessonal(Professionalsvm vm)
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


    }
}
