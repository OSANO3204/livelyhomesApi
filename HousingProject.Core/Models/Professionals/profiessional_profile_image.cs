using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Professionals
{
    public class profiessional_profile_image
    {
        [Key]
        public int Professional_profile_image_id { get; set; }
        public string  WorkerId { get; set; }
        public byte[] Data { get; set; }
        public string FileName { get; set; }
        public DateTime Date_Uploaded { get; set; } = DateTime.Now;
    }
}
