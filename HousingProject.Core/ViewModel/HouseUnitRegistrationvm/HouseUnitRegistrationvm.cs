using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.HouseUnitRegistrationvm
{
 public     class HouseUnitRegistrationvm
    {

        public int HouseID { get; set; }
        public int HouseUnitNumber { get; set; }
        public bool Occupied { get; set; }
        public bool Vacant { get; set; }
        public string HouseUnitFloor { get; set; }
        public string GeneratedId { get; set; }
    }
}
