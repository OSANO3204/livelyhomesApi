using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HousingProject.Core.ViewModel.House.HouseUsersvm
{
    public    class HouseUsersViewModel
    {

        public string FirstName { get; set; }


        public string LasstName { get; set; }

      
        public string IdNumber { get; set; }
        public string Salutation { get; set; }

        public string Gender { get; set; }

        public string BirthDate { get; set; }

        public int HouseID { get; set; }
        public string HouseName { get; set; }

        public string Email { get; set; }



        public string PhoneNumber { get; set; }



    }
}
