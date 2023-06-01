using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.ImagesVm;
using HousingProject.Infrastructure.ExtraFunctions.Images;
using HousingProject.Infrastructure.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HousingProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesServices _imagesServices;
        public ImagesController(IImagesServices imagesServices)
        {
            _imagesServices = imagesServices;
        }

        [Authorize]
        [HttpPost]
        [Route("UploadImage")]
     
        public async Task<BaseResponse> UploadImages( List<IFormFile> ifiles, string  uploadReason, string useremail)
        {    
            return await _imagesServices.UploadImages(ifiles, uploadReason, useremail);

        }


        [Authorize]
        [HttpPost]
        [Route("getprofileImage")]
        public async Task<BaseResponse> GetprofileImage(string profiledescription, string userEmail)
        {
            return await _imagesServices.GetprofileImage(profiledescription, userEmail);
        }


        [Authorize]
        [HttpGet]
        [Route("GetAllImages")]
        public async Task<imageresponse> GetAllImages()
        {

            return await _imagesServices.GetAllImages();
        }

    }
}
