using HousingProject.Architecture.ViewModel.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HousingProject.Architecture.ViewModel.People
{

    public class RegisterViewModel
    {
        [Key]

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


        [Required]
        [DisplayName("IsHouseUsers")]
        public bool IsHouseUsers { get; set; }


        [DisplayName("Email"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Retype-Password"), DataType(DataType.Password)]
        public string RetypePassword { get; set; }

        public bool Is_Tenant { get; set; }

       
        public int TenantId { get; set; }

        [DisplayName("RentPayday"), DataType(DataType.Password)]
        public DateTime RentPayDay { get; set; }

    }
}
