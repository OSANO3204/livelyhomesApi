using HousingProject.Core.Models.Houses.Flats.House_Registration;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.People  
{
    public class RegistrationModel: IdentityUser
    {
       // [Key]
      //  public int Us

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LasstName { get; set; }

        [Required]
        [DisplayName("IsHouseUsers")]
        public bool IsHouseUsers { get; set; }


        [Required]
        [DisplayName("ID Number")]
        public string IdNumber { get; set; }

        [Required]
        [DisplayName("Date Of Birth")]
        public string BirthDate { get; set; }


        [DataType("DateTime")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string Salutation { get; set; }

        public string Gender { get; set; }
        public bool Is_Landlord { get; set; }

        public bool Is_CareTaker { get; set; }
        public string VerificationToken { get; set; }
        public bool Is_Agent { get; set; }
        public bool Is_Tenant { get; set; }
        public bool Is_Admin { get; set; }
        public int TenantId { get; set; }
    
        public List<House_Registration> Houses_Registered { get; set; }


    }
}
