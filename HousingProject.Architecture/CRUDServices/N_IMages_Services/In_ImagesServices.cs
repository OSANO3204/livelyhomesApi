using HousingProject.Architecture.Response.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.CRUDServices.N_IMages_Services
{
   public  interface In_ImagesServices
    {
         Task<BaseResponse> AddImages(IFormFile file);
          Task<BaseResponse> Get_All_Images();
         Task<BaseResponse> GetImageById(int id);
    }
}
