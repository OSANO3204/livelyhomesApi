using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Houses.Flats.AdminContacts
{
  public   class AdminContacts
    {
        [Key]
        public int contactsId { get; set; }
        public string  AdminEmail { get; set; }
        public string Creator { get; set; }
        public string AdminPhoneNumber { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

    }
}
