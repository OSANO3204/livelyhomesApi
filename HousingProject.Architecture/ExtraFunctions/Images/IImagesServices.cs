using HousingProject.Architecture.Response.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions.Images
{
    public  interface IImagesServices
    {
        Task<BaseResponse> UploadImages(List<IFormFile> ifiles, string uploadReason, string useremail);
        Task<BaseResponse> GetprofileImage(string profiledescription, string userEmail);
    }
}
