
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HousingProject.Core.ViewModel.ImagesVm
{
    public class ImaageUploadClassvm
    {

        public string Imagename { get; set; }
  

        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "jpg,png,gif,jpeg,bmp,svg")]
        public IFormFile image { get; set; }
    }
}
