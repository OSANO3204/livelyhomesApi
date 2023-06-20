using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.N_IMAGES;
using HousingProject.Core.ViewModel.n_Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.CRUDServices.N_IMages_Services
{
    public class n_images_services: In_ImagesServices
    {
        private readonly HousingProjectContext _context;
        private readonly IServiceScopeFactory _servicescope;
        public n_images_services(
            HousingProjectContext context,
            IServiceScopeFactory servicescope
            )
        {
            _context = context;
            _servicescope =servicescope ;
        }

        public async Task<BaseResponse> AddImages(IFormFile file)
        {
            try
            {
                using (var scope = _servicescope.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    if (file == null || file.Length == 0)
                    {
                        return new BaseResponse { Code = "190", ErrorMessage = "No file uploaded." };
                    }

                        var image = new Image_Models();
                        image.FileName = Path.GetFileName(file.FileName);

                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            image.Data = memoryStream.ToArray();
                        }

                        scopedcontext.Image_Models.Add(image);
                        await scopedcontext.SaveChangesAsync();
                        return new BaseResponse { Code = "200", ErrorMessage = "Image uploaded successfully." };
                }
            
            }

            catch (Exception ex)
            {
                return new BaseResponse { Code = "150", ErrorMessage = $"An error occurred: {ex.Message}" };
            }
        }



        public async Task<BaseResponse> Get_All_Images()
        {
            try
            {
                using (var scope = _servicescope.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                 
                        var images = await scopedcontext.Image_Models.ToListAsync();

                    List<iamde_modelvm> imageslist = new List<iamde_modelvm>();
                    List<string> base64Images = new List<string>();
                    foreach (var data in images)
                    {
                        var imagedata = new iamde_modelvm
                        {
                            Data = data.Data,
                            filestring = Convert.ToBase64String(data.Data),
                            FileName = data.FileName
                        };
                        imageslist.Add(imagedata);                           
                    }

                    return new BaseResponse {Code="200",SuccessMessage="successfully queried", Body = imageslist };      
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "140", ErrorMessage = $"An error occurred: {ex.Message}" };
            }

        }

     
        public async Task<BaseResponse> GetImageById(int id)
        {
            try
            {
                using (var scope = _servicescope.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var image = await scopedcontext.Image_Models.FindAsync(id);

                    if (image == null)
                    {
                        return new BaseResponse();
                    }

                    return  new BaseResponse { Body = image.Data, SuccessMessage = "image/jpeg" };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code="190", ErrorMessage= $"An error occurred: {ex.Message}" };
            }
        }


    }
}
