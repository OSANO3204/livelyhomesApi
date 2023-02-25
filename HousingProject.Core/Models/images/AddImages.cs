using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HousingProject.Core.Models.images
{
    public  class AddImages
    {
       

        [Required(ErrorMessage = "Please Enter Avatar Url")]
        public IFormFile DisplayImage { get; set; }
    }
}
