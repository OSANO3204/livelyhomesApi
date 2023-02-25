using HousingProject.Core.Models.Houses.Flats.House_Registration;
using HousingProject.Core.Models.People.Landlord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.People.Tenants
{
   public  class Summarystatementsvm
    {
        public string AgentName { get; set; }
        public int HouseDoornumber { get; set; }
        public float HouseRent { get; set; }
        public int FlatNumberId { get; set; }
        public string DateOfRentPayment { get; set; }
        public float RentArrears { get; set; }
        public float overpayment { get; set; }
        public bool RentPaid { get; set; }
        public bool OverDueRent { get; set; }

        public bool IsLandlord { get; set; }

        public int TenantId { get; set; }
        public int HouseiD { get; set; }
        public House_Registration Houseregistration { get; set; }

        public int LandlordId { get; set; }
        public Landlordmodel Landlord { get; set; }
    }
}
