using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.Professionalsvm;
using HousingProject.Infrastructure.Interfaces.IProfessionalsServices;
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


    }
}
