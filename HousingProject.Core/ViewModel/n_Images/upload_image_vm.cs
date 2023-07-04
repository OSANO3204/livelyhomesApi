using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.n_Images
{
  public   class upload_image_vm
    {
        public IFormFile file { get; set; }
        public string FileName { get; set; }
 
        public string Image_Description { get; set; }
    }
}
