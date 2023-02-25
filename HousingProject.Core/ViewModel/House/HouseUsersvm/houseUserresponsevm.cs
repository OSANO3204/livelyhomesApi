using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.House.HouseUsersvm
{
  public  class houseUserresponsevm
    {
        public string FirstName { get; set; }
    
        public string LasstName { get; set; }
 
        public string IdNumber { get; set; }
        public string Salutation { get; set; }
        public string Gender { get; set; }

        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string HouseName { get; set; }
        public bool AccountActivated { get; set; }
        public int HouseID { get; set; }
        public string Creatormail { get; set; }

        public int CreatorId { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Password { get; set; }


        public string RetypePassword { get; set; }


    }
}
