using HousingProject.Core.Models.Houses.Flats.House_Registration;
using HousingProject.Core.Models.People.Landlord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.General
{
   public  class TenantSummary
    {
        [Key]
        public int SummaryId { get; set; }
        public string AgentName { get; set; }
        public int HouseDoornumber { get; set; }
        public float HouseRent { get; set; }
        public int FlatNumberId { get; set; }
        public string DateOfRentPayment { get; set; }
        public float RentArrears { get; set; }
        public float overpayment { get; set; }
        public bool RentPaid { get; set; }
        public bool OverDueRent { get; set; }
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        public bool IsLandlord { get; set; }
        public int TenantId { get; set; }
        public int HouseiD { get; set; }
        public House_Registration Houseregistration { get; set; }
        public int LandlordId { get; set; }
        public Landlordmodel Landlord { get; set; }

    }
}
