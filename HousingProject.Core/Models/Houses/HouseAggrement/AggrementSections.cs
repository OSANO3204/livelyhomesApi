using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Houses.HouseAggrement
{
  public   class AggrementSections
    {
        [Key]
        public int AggreementfieldsId { get; set; }

        public bool show_LandlordName { get; set; }
        public string  LandlordName { get; set; }

        public bool show_TenantNmae { get; set; }
        public string TenantNmae { get; set; }

        public bool show_AgentName { get; set; }
        public string AgentName { get; set; }


        public bool show_HouseName { get; set; }
        public string HouseName { get; set; }

        public bool show_HouseLocation { get; set; }
        public string HouseLocation { get; set; }

        public bool show_SecurityDeposit { get; set; }
        public string  SecurityDeposit { get; set; }

        public bool show_ServiceFee { get; set; }
        public string ServiceFee { get; set; }

        public bool show_Maintainance_and_Repairs { get; set; }
        public string Maintainance_and_Repairs { get; set; }

        public bool show_Rent_Increased_After_in_years { get; set; }
        public  string Rent_Increased_After_in_years { get; set; }

        public bool show_Increasepercentage { get; set; }
        public string Increasepercentage { get; set; }

        public bool show_Increase_flat_rate { get; set; }
        public string Increase_flat_rate { get; set; }

        public bool show_Other_Aggreement { get; set; }
        public string Other_Aggreements { get; set; }

   
    }

}
