using System;
using System.ComponentModel.DataAnnotations;

namespace HousingProject.Core.Models.Houses.HouseUnitRegistration
{
    public   class HouseUnit
    {
        [Key]
        public Guid HouseunitId { get; set; }
        public int HouseID { get; set; }
        public int HouseUnitNumber { get; set; }
        public bool Occupied { get; set; }
        public bool  Vacant { get; set; }

        public string GeneratedId { get; set; }
        public string HouseUnitFloor { get; set; }

        


    }       
}
