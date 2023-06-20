using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.ImagesVm;
using HousingProject.Infrastructure.CRUDServices.N_IMages_Services;
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
        private readonly In_ImagesServices _n_imageservices;
        public ImagesController(IImagesServices imagesServices, In_ImagesServices n_imageservices)
        {
            _imagesServices = imagesServices;
            _n_imageservices = n_imageservices;

        }

        //[Authorize]
        //[HttpPost]
        //[Route("UploadImage")]
     
        //public async Task<BaseResponse> UploadImages( List<IFormFile> ifiles, string  uploadReason, string useremail)
        //{    
        //    return await _imagesServices.UploadImages(ifiles, uploadReason, useremail);

        //}


        //[Authorize]
        //[HttpPost]
        //[Route("getprofileImage")]
        //public async Task<BaseResponse> GetprofileImage(string profiledescription, string userEmail)
        //{
        //    return await _imagesServices.GetprofileImage(profiledescription, userEmail);
        //}


        //[Authorize]
        //[HttpGet]
        //[Route("GetAllImages")]
        //public async Task<imageresponse> GetAllImages()
        //{

        //    return await _imagesServices.GetAllImages();
        //}

        [Authorize]
        [HttpPost]
        [Route("AddImage")]
        public async Task<BaseResponse> AddImages(IFormFile file)
        {

            return await _n_imageservices.AddImages(file);
        }

        [Authorize]
        [HttpPost]
        [Route("Get_n_Images_By_Id")]
        public async Task<BaseResponse> GetImage(int id)
        {

            return await _n_imageservices.GetImageById(id);
        }

        [Authorize]
        [HttpGet]
        [Route("GetAll_n_Images")]
        public async Task<BaseResponse> Get_All_Images()
        {

            return await _n_imageservices.Get_All_Images();
        }
    }
}
