using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Houses.HouseAggrement
{
   public  class Aggrement
    {
        [Key]
        public int AggreementID { get; set; }
        public int HouseID { get; set; }
        public bool EnforceAggreement { get; set; }
        public string LandlordName { get; set; }
        public string Agent { get; set; }
        public string CreatedBy { get; set; }
        public bool AggreeToAggreement { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
