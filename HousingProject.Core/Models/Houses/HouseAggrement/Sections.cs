using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Houses.HouseAggrement
{
   public  class Sections
    {
        [Key]
        public int AggreementSectiondID { get; set; }
        public string SectionName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsTrue { get; set; }
    }
}
