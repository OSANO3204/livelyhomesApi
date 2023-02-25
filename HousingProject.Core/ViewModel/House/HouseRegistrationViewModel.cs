using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModels
{
   public  class HouseRegistrationViewModel
    {
        public string House_Location { get; set; }
        public int HouseiD { get; set; }
        public int Total_Units { get; set; }

        public string Owner_Firstname { get; set; }

        public string Owner_LastName { get; set; }

        public int Owner_id_Number { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }

        public string House_Name { get; set; }

        public int UserId { get; set; }

        public int Estimated_Maximum_Capacity { get; set; }
    }
}
