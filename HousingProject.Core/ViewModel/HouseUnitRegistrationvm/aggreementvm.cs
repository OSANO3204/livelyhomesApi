using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.HouseUnitRegistrationvm
{
  public   class aggreementvm
    {

        public int HouseID { get; set; }
        public bool EnforceAggreement { get; set; }
        public string LandlordName { get; set; }
        public string AgentName { get; set; }
        public bool AggreeToAggreement { get; set; }

    }
}
