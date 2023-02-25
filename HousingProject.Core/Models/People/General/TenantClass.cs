using HousingProject.Core.Models.General;
using HousingProject.Core.Models.Houses.Flats.House_Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.People.General
{
   public  class TenantClass
    {

        [Key]
        public int RenteeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string HouseFloor { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public int Cars { get; set; }

        public int ServicesFees { get; set; }

        public string Rentee_PhoneNumber { get; set; }

        public int BedRoom_Number { get; set; }

        public bool Email_Confirmed { get; set; } = false;

        public int Number0f_Occupants { get; set; }

        public float House_Rent { get; set; }
        public float RentPaid { get; set; }
        public float RentArrears { get; set; }
        public float  RentOverpayment { get; set; }

        public string Agent_PhoneNumber { get; set; }

        public string BuildingCareTaker_PhoneNumber { get; set; }

        public int Appartment_DoorNumber { get; set; }

        public int HouseiD { get; set; }
        public House_Registration House_Registration { get; set; }

        public List<TenantSummary> Summary { get; set; }
        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;


        [DisplayName("RentPayday"), DataType(DataType.Password)]
        public DateTime RentPayDay { get; set; }



        [DisplayName("CurrentMonthrent"), DataType(DataType.Password)]
        
        public Double currentMonthRent { get; set; }
    }
}
