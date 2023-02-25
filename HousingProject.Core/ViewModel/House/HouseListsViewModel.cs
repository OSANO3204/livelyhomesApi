using HousingProject.Core.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.House
{
  public   class HouseListsViewModel
    {
        public string House_Location { get; set; }
        public int Total_Units { get; set; }
        public int HouseiD { get; set; }
        public string Owner_Firstname { get; set; }

        public string Owner_LastName { get; set; }

        public int Owner_id_Number { get; set; }
        

        public string House_Name { get; set; }

        public int UserId { get; set; }
        public RegistrationModel CreatedBy { get; set; }
        public int Estimated_Maximum_Capacity { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public bool EmailSent { get; set; }

        public string Country { get; set; }
        public string Area { get; set; }
        public string CreatorNames { get; set; }
        public string CreatorEmail { get; set; }

        public string HouseImage { get; set; }
        public string ImageName { get; set; }

        public string responsemessage { get; set; }

        public string ErrorMessage { get; set; }
    }
}
