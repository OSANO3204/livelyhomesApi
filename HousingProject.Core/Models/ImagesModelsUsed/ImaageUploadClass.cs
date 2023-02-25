using System;
using System.ComponentModel.DataAnnotations;

namespace HousingProject.Core.Models.ImagesModelsUsed
{
    public class ImaageUploadClass
    {

        [Key]
        public int imagedId { get; set;}
        public  string Description{get; set;}
        public string CreatedBy { get; set;}
        public string ImgeName { get; set;}
        public string ImagePath { get; set;}
        public  DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UserEmail { get; set; }
    }
}
