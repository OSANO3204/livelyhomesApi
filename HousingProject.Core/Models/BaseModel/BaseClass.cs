using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Architecture.Models.People
{
    public  class BaseClass
    {

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LasstName { get; set; }

        [Required]
        [DisplayName("ID Number")]
        public string IdNumber { get; set; }

        [Required]
        [DisplayName("Date Of Birth")]
        public string BirthDate { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [DataType("DateTime")]

        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
}
