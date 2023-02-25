using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.People.GeneralRegistration
{
  public   class IdentityRegistrationVm
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Retype-Password"), DataType(DataType.Password)]
        public string RetypePassword { get; set; }

    }
}
