using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.images
{
    public  class addImageModel
    {
        [Key]

        public int imagedId { get; set; }
        public object  UploadedImage { get; internal set; }
    }
}
