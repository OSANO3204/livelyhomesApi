using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Professionals
{
   public  class Add_Services
    {
        [Key]
        public int Service_Id { get; set; }

        public string Service { get; set; }
        public string Job_Number { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
