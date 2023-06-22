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
        public string Agent { get; set; }
        public string LeastStartDate { get; set; }
        public string HouseLocation { get; set; }
        public string LeastEndDateDate { get; set; }

   

        public decimal RentAmount { get; set; }

        public Decimal MaintainceAndRepairDeposit { get; set; }

        public int RentIncreasePeriod { get; set; }

        public int RentDepositAmount { get; set; }

        public int Rentincreasepercentage { get; set; }

        public int Renincreaseflatrate { get; set; }

        public decimal Serviceffeedeposit { get; set; }

        public string AnyOtherTerms { get; set; }
        public bool AggreeToAggreement { get; set; }
        public string TenantName { get; set; }
        public string TenantEmail { get; set; }
        public bool AggreementStatus { get; set; }
    }
}
