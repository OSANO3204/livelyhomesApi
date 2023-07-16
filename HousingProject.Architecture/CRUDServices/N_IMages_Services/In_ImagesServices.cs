using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.n_Images;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.CRUDServices.N_IMages_Services
{
    public  interface In_ImagesServices
    {
         Task<BaseResponse> AddImages(IFormFile file);
         Task<BaseResponse> Get_All_Images();
         Task<BaseResponse> GetImageById(int id);
         Task<BaseResponse> Add_Profile_Pics(IFormFile file, string Image_Description);
         Task<BaseResponse> Get_User_Profile_Image();
         Task<BaseResponse> Add_House_Profile_Image(IFormFile file, int houseid);
         Task<BaseResponse> Get_House_Profile_Image(int house_id);
        Task<BaseResponse> upload_Technician_Profile_Image(IFormFile file, string workerid);
        Task<BaseResponse> Get_Technician_Profile_Image(string worker_id);
         Task<BaseResponse> Get_User_Profile_Image_with_user_email(string user_email);
    }
}
