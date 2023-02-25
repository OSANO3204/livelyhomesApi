using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions
{
    public class AddCities
    {
        [Key]
        public int SubCountyId { get; set; }

        public int CountyId { get; set; }

        public string CityName { get; set; }
    }
}
