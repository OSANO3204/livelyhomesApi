using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.HouseUnitRegistrationvm
{
   public class Housing_Profile_vm
    {
        public string House_Location { get; set; }
        public int Total_Units { get; set; }
        public string Owner_Firstname { get; set; }
        public string Owner_LastName { get; set; }
        public int Owner_id_Number { get; set; }    
        public string House_Name { get; set; }
        public string UserId { get; set; }   
        public int Estimated_Maximum_Capacity { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool EmailSent { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string CreatorNames { get; set; }
        public string CreatorEmail { get; set; }
        public int Total_Monthly_Rent { get; set; }
        public int Total_Occupied_Units { get; set; }
        public int Total_UnOccupied_Units { get; set; }

    }
}
