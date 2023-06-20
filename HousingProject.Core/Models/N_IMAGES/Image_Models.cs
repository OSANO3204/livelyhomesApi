using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.N_IMAGES
{
   public  class Image_Models
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public byte[] Data { get; set; }
    }
}
