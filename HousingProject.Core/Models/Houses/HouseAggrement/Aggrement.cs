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
        public string  HouseLocation { get; set; }
        public string Agent { get; set; }
        public DateTime LeastStartDate { get; set; }

        public DateTime LeastEndDateDate { get; set; }

        public string CreatedBy { get; set; }

        public decimal RentAmount { get; set; }

        public Decimal MaintainceAndRepairDeposit { get; set; }

        public int RentIncreasePeriod { get; set; }

        public int RentDepositAmount { get; set; }

        public int Rentincreasepercentage { get; set; }

        public int Renincreaseflatrate { get; set; }


        public decimal Serviceffeedeposit { get; set; }


        public string  AnyOtherTerms { get; set; }
        public bool AggreeToAggreement { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
