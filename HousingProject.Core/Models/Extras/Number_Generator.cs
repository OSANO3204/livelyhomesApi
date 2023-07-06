using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Extras
{
    public class Number_Generator
    {
        [Key]
        public int Number_Generator_id { get; set; }
        public int Generated_Number { get; set; }
        public DateTime DateUpdated { get; set; } = DateTime.Now;
    }
}
