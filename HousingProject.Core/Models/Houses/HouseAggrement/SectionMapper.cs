using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Houses.HouseAggrement
{
  public   class SectionMapper
    {
        [Key]
        public int SectionMapperID { get; set; }
        public int AggreemenID { get; set; }
        public int AggreementSectionID { get; set; }


    }
}
