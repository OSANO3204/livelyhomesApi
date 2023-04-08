using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Houses
{
 public   class Aggreement_Sections_Mapper
    {
        [Key]
        public int ggreement_sections_mapperID { get; set; }
        public int AggrementID { get; set; }
        public int Aggrement_sections_fields { get; set; }
    }
}
