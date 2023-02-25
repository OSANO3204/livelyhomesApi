using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HousingProject.Core.Models.Houses.HouseUsers
{
    public   class HouseUsers
    {
        [Key]
        public int HouseuserId { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LasstName { get; set; }

        [DisplayName("ID Number")]
        public string IdNumber { get; set; }
        public string Salutation { get; set; }

        public string Gender { get; set; }

        [DisplayName("Date Of Birth")]
        public string BirthDate { get; set; }

        [DisplayName("Email"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string HouseName { get; set; }
        public bool AccountActivated { get; set; }
        public int HouseID { get; set; }
        public string  Creatormail { get; set; }

        public int CreatorId { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Password { get; set; }

        [DisplayName("Retype-Password"), DataType(DataType.Password)]
        public string RetypePassword { get; set; }





    }
}
