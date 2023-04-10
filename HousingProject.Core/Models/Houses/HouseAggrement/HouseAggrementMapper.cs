using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Houses.HouseAggrement
{
    public class HouseAggrementMapper
    {
        [Key]
        public int Aggreement_sections_mapperID { get; set; }
        public int AggrementID { get; set; }
        public int Aggrement_sections_fields { get; set; }
    }
}
