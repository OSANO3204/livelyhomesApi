using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.CountiesModel
{
  
    public class AddCounty
    {
        [Key]
        public int CountyId { get; set; }
        public string CountyName { get; set; }
        public string AddedBy { get; set; }
        public int CreatorID { get; set; }
        public DateTime MyProperty { get; set; } = DateTime.Now;
    }
}
