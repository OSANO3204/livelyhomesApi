using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.CountiesModel
{
   public class AddCountyArea
    {
        [Key]
        public int CountyAreaId { get; set; }

        public string CountyArea { get; set; }

        public int CountyId { get; set; }
    }
}
