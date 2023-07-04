using System.ComponentModel.DataAnnotations;

namespace HousingProject.Core.Models.Houses
{
    public  class House_Profile_Image
    {
        [Key]
        public int House_Image_Id { get; set; }

        public int House_Id { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public byte[] Data { get; set; }
        
    }
}
