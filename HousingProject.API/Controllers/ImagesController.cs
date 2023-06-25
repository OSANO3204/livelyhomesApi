using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.ImagesVm;
using HousingProject.Core.ViewModel.n_Images;
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

        //public async Task<BaseResponse> UploadImages(List<IFormFile> ifiles, string uploadReason, string useremail)
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

        [Authorize]
        [HttpPost]
        [Route("Add_User_Profile_Pic")]
        public async Task<BaseResponse> Add_Profile_Pics(IFormFile file , string Image_Description)
        {
            return await _n_imageservices.Add_Profile_Pics(file, Image_Description);
        }

        [Authorize]
        [HttpGet]
        [Route("Get_user_profile_pic")]
        public async Task<BaseResponse> Get_User_Profile_Image()
        {
            return await _n_imageservices.Get_User_Profile_Image();
        }

        [Authorize]
        [HttpPost]
        [Route("Add_House_Profile_Image")]
        public async Task<BaseResponse> Add_House_Profile_Image(IFormFile file, int houseid)
        {
            return await _n_imageservices.Add_House_Profile_Image(file, houseid);
        }

        [Authorize]
        [HttpPost]
        [Route("Get_House_Profile_Id")]

        public async Task<BaseResponse> Get_House_Profile_Image(int house_id) 
            {
            return await _n_imageservices.Get_House_Profile_Image(house_id);

            }

        [Authorize]
        [HttpPost]
        [Route("Upload_Technician_Profile_Image")]
        public async Task<BaseResponse> upload_Technician_Profile_Image(IFormFile file, string workerid)
        {
            return await _n_imageservices.upload_Technician_Profile_Image(file, workerid);
        }

        [Authorize]
        [HttpPost]
        [Route("Get_Technician_Profile_Image")]

        public async Task<BaseResponse> Get_Technician_Profile_Image(string worker_id)
        {
            return await _n_imageservices.Get_Technician_Profile_Image(worker_id);

        }

    }
}
