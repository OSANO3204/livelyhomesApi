using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Houses.Flats.uploadImage
{
public class UploadImage
    {
        [Key]
        public int ImageId { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }      
    }
}

