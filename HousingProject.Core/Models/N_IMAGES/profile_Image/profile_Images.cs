using System;
using System.ComponentModel.DataAnnotations;

namespace HousingProject.Core.Models.N_IMAGES.profile_Image
{
   public  class profile_Images
    {
        [Key]
        public int profile_id { get; set; }
        public string userid { get; set; }
        public string FileName { get; set; } 
        public byte[] Data { get; set; }
        public string Image_Description { get; set; }
        public DateTime Date_Uploaded { get; set; } = DateTime.Now;
    }
}
