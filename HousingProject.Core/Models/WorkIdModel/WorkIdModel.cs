using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.WorkIdModel
{
   public  class WorkIdModel
    {
        [Key]

        public int WorkIdKey { get; set; }

        public int WorkIdSaved { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
