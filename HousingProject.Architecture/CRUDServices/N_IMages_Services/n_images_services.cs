using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Houses;
using HousingProject.Core.Models.N_IMAGES;
using HousingProject.Core.Models.N_IMAGES.profile_Image;
using HousingProject.Core.Models.Professionals;
using HousingProject.Core.ViewModel.n_Images;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.CRUDServices.N_IMages_Services
{
    public class n_images_services : In_ImagesServices
    {
        private readonly HousingProjectContext _context;
        private readonly IServiceScopeFactory _servicescope;
        private readonly ILoggedIn _iloogedinservices;

        public n_images_services(
            HousingProjectContext context,
            ILoggedIn iloogedinservices,
            IServiceScopeFactory servicescope
            )
        {
            _context = context;
            _servicescope = servicescope;
            _iloogedinservices = iloogedinservices;
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

                    return new BaseResponse { Code = "200", SuccessMessage = "successfully queried", Body = imageslist };
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

                    return new BaseResponse { Body = image.Data, SuccessMessage = "image/jpeg" };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "190", ErrorMessage = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<BaseResponse> Add_Profile_Pics(IFormFile file, string Image_Description)
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
                    var user = _iloogedinservices.LoggedInUser().Result;
                    var image = await scopedcontext.profile_Images.Where(y => y.Image_Description != ""  && y.userid==user.Id).FirstOrDefaultAsync();
                    

                    if (image == null)
                    {

                        var newimage = new profile_Images();

                        newimage.FileName = Path.GetFileName(file.FileName);
                        newimage.userid = user.Id;
                        newimage.Image_Description = Image_Description;
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            newimage.Data = memoryStream.ToArray();
                        }
                        await scopedcontext.AddAsync(newimage);
                        await scopedcontext.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "Profile Image Uploaded successfully" };
                    }
                    else
                    {
                        image.FileName = Path.GetFileName(file.FileName);
                        image.userid = user.Id;
                        image.Image_Description = Image_Description;
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            image.Data = memoryStream.ToArray();
                            scopedcontext.Update(image);
                            await scopedcontext.SaveChangesAsync();
                            return new BaseResponse { Code = "200", SuccessMessage = "Profile Image updated successfully" };
                        }
      

                    }
                   
                }

            }
            catch (Exception ex)
            {

                return new BaseResponse { Code = "140", ErrorMessage = ex.Message };
            }


        }

        public async Task<BaseResponse> Get_User_Profile_Image()
        {

            try
            {
                using (var scope = _servicescope.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    var user = _iloogedinservices.LoggedInUser().Result;

                    var profile_Image_obj = await scopedcontext.profile_Images.Where(y => y.userid == user.Id).FirstOrDefaultAsync();

                   
                    if (profile_Image_obj == null)
                    {
                        return new BaseResponse();
                    }

                    return new BaseResponse { Body = profile_Image_obj.Data, SuccessMessage = "Image data  Queried successfully " };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "190", ErrorMessage = $"An error occurred: {ex.Message}" };
            }

        }


       public async Task<BaseResponse> Add_House_Profile_Image(IFormFile file, int houseid)
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
                    var user = _iloogedinservices.LoggedInUser().Result;
                    var image = await scopedcontext.House_Profile_Image.Where( y=>y.House_Id == houseid).FirstOrDefaultAsync();


                    if (image == null)
                    {

                        var house_profile = new House_Profile_Image();

                        house_profile.FileName = Path.GetFileName(file.FileName);
                        house_profile.House_Id = houseid;
                       
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            house_profile.Data = memoryStream.ToArray();
                        }
                        await scopedcontext.AddAsync(house_profile);
                        await scopedcontext.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "House Profile Image Uploaded successfully" };
                    }
                    else
                    {
                        image.FileName = Path.GetFileName(file.FileName);
                        image.House_Id = houseid;
                       
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            image.Data = memoryStream.ToArray();
                            scopedcontext.Update(image);
                            await scopedcontext.SaveChangesAsync();
                            return new BaseResponse { Code = "200", SuccessMessage = "House Profile Image updated successfully" };
                        }


                    }

                }

            }
            catch (Exception ex)
            {

                return new BaseResponse { Code = "140", ErrorMessage = ex.Message };
            }

        }

        public async Task<BaseResponse> Get_House_Profile_Image(int house_id)
        {
            try
            {
                using (var scope = _servicescope.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var image = await scopedcontext.House_Profile_Image.FindAsync(house_id);

                    if (image == null)
                    {
                        return new BaseResponse();
                    }

                    return new BaseResponse { Body = image.Data, SuccessMessage="House profile image queried successfully"};
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "190", ErrorMessage = $"An error occurred: {ex.Message}" };
            }
        }


        //technician profile 
        public async Task<BaseResponse> upload_Technician_Profile_Image(IFormFile file, string workerid)
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
                    var user = _iloogedinservices.LoggedInUser().Result;
                    var image = await scopedcontext.profiessional_profile_image.Where(y => y.WorkerId == workerid).FirstOrDefaultAsync();


                    if (image == null)
                    {

                        var technician_profile = new profiessional_profile_image();

                        technician_profile.FileName = Path.GetFileName(file.FileName);
                        technician_profile.WorkerId = workerid;

                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            technician_profile.Data = memoryStream.ToArray();
                        }
                        await scopedcontext.AddAsync(technician_profile);
                        await scopedcontext.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "Technician Profile Image Uploaded successfully" };
                    }
                    else
                    {
                        image.FileName = Path.GetFileName(file.FileName);
                        image.WorkerId = workerid;

                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            image.Data = memoryStream.ToArray();
                            scopedcontext.Update(image);
                            await scopedcontext.SaveChangesAsync();
                            return new BaseResponse { Code = "200", SuccessMessage = "Technician Profile Image updated successfully" };
                        }


                    }

                }

            }
            catch (Exception ex)
            {

                return new BaseResponse { Code = "140", ErrorMessage = ex.Message };
            }

        }



        public async Task<BaseResponse> Get_Technician_Profile_Image(string worker_id)
        {
            try
            {
                using (var scope = _servicescope.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var image = await scopedcontext.profiessional_profile_image.Where(x=>x.WorkerId== worker_id).FirstOrDefaultAsync();
                    if (image == null)
                    {
                        return new BaseResponse();
                    }

                    return new BaseResponse { Body = image.Data, SuccessMessage = "Technician profile image queried successfully" };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "190", ErrorMessage = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<BaseResponse> Get_User_Profile_Image_with_user_email(string user_email)
        {

            try
            {
                using (var scope = _servicescope.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();



                    var found_user = await scopedcontext.RegistrationModel.Where(y => y.Email == user_email).FirstOrDefaultAsync();
                    if (found_user == null) new BaseResponse { ErrorMessage = "No user profile image" };

                    var profile_Image_obj = await scopedcontext.profile_Images.Where(y => y.userid == found_user.Id).FirstOrDefaultAsync();


                    if (profile_Image_obj == null)
                    {
                        return new BaseResponse();
                    }

                    return new BaseResponse { Body = profile_Image_obj.Data, SuccessMessage = "Image data  Queried successfully " };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "190", ErrorMessage = $"An error occurred: {ex.Message}" };
            }

        }



    }
}

