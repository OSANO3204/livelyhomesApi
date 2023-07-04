using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.ImagesModelsUsed;
using HousingProject.Infrastructure.ExtraFunctions.Checkroles.IcheckRole;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
using HousingProject.Infrastructure.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;




namespace HousingProject.Infrastructure.ExtraFunctions.Images
{

    public   class ImagesServices: IImagesServices
    {
        private readonly ICheckroles _checkroles;
        public static IHostingEnvironment _environment;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly HousingProjectContext  _context;
        public readonly ILoggedIn _loggedIn;
        private readonly IServiceScopeFactory _servicescope;
        private readonly IUrlHelper _urlhelper;
      

        public ImagesServices(
            ICheckroles checkroles,
            IHttpContextAccessor httpContextAccessor,
            HousingProjectContext context,
            IHostingEnvironment environment,
            ILoggedIn loggedIn,
            IServiceScopeFactory servicescope,
            IUrlHelper urlhelper
            )
        {        
            _environment = environment;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _checkroles = checkroles;
            _loggedIn = loggedIn;
            _servicescope = servicescope;
            _urlhelper = urlhelper;
        }

        public async Task<BaseResponse> UploadImages( List<IFormFile> ifiles, string  uploadReason, string useremail)
        {
            var user = _loggedIn.LoggedInUser().Result;
            if (ifiles == null || ifiles.Count==0)
            {
                return new BaseResponse { Code = "234", ErrorMessage = "No image selected" };
            }
            foreach (var ifile in ifiles)
            {
                string imagetext = Path.GetExtension(ifile.FileName);
                if (imagetext == ".jpg" || imagetext == ".gif" || imagetext == ".jpeg"  ||imagetext== ".png")
                {
                    var saveimage = Path.Combine(_environment.WebRootPath, "Images", ifile.FileName);
                    var stream = new FileStream(saveimage, FileMode.Create);
                    await ifile.CopyToAsync(stream);
                    var saveImage = new ImaageUploadClass

                    {
                        ImgeName = ifile.FileName,
                        ImagePath = saveimage,
                        Description=uploadReason,
                        CreatedBy= user.Email,
                        UserEmail= useremail
                    };
                    await _context.AddAsync(saveImage);
                    await _context.SaveChangesAsync();
                    return new BaseResponse {Code = "200",SuccessMessage = "Image uploaded successfully" };
                }
            }          
        return new BaseResponse { Code = "140", ErrorMessage = "Something foreign happened" };
        }

        public async Task<BaseResponse> GetprofileImage(string profiledescription, string userEmail)
        {
            if (profiledescription =="")
            {
                return new BaseResponse { Code = "123", ErrorMessage = "Description cannot be empty" };               
            }
            if (userEmail == "")
            {
                return new BaseResponse { Code = "467", ErrorMessage = "Email cannot be empty" };
            }

            try
            {
              var imageuploaded =await  _context.ImaageUploadClass.Where(x => x.UserEmail == userEmail && x.Description == profiledescription).FirstOrDefaultAsync();
              if (imageuploaded == null)
                {
                    return new BaseResponse { Code = "346", ErrorMessage = "The profile image cannot be found " };
                }
                var ImagePath=_environment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "Images" + Path.DirectorySeparatorChar.ToString() + imageuploaded.ImagePath;
                return new BaseResponse { Code = "200", SuccessMessage = ImagePath };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "467", ErrorMessage = ex.ToString() };
            }
        }
        public async Task<imageresponse> GetAllImages()
        {
           try
            {
                using (var scope = _servicescope.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider
                        .GetRequiredService<HousingProjectContext>();
                    ////start 

                    string webRootPath = _environment.WebRootPath;
                    string imagesPath = Path.Combine(webRootPath, "Images\\");
                    string[] imageFiles = Directory.GetFiles(imagesPath);
                    var imageUrls = new List<string>();
                    foreach (var imagePath in imageFiles)
                    {
                        var imageUrl = _urlhelper.Content("localhost:2023/"+ imagesPath + Path.GetFileName(imagePath));
                        imageUrls.Add(imageUrl);
                    }
                    return new imageresponse { message = "Successfully queried", imagepaths = imageUrls };
                    ////end
                    ///

                    //var allimagesfound = await scopedcontext.ImaageUploadClass.ToListAsync();

                    //return new imageresponse { message = "Queried successfully", imagepaths=allimagesfound };
                }

            }
            catch (Exception ex)
            {
                return new imageresponse {message = ex.Message };
            }
        }



    }
}
