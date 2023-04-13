using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.People.General
{
   public  class ContactUs
    {
        [Key]
        public int ContacusId { get; set; }

        public string Message_title { get; set; }
        public string Useremail { get; set; }
        public string UserMessage { get; set; }
        public bool  ClosedMessages { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

    }
}
