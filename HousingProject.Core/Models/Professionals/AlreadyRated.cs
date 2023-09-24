using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Professionals
{
   public  class AlreadyRated
    {
        [Key]
        public int RatedID { get; set; }
        public string Email { get; set; }
        public bool NotRated { get; set; } = true;
        public string TechincianID { get; set; }
        public bool Message { get; set; }
    }
}
