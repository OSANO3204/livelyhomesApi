using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Professionals
{
  public   class RegisterProfessional
    {
        [Key]
        public int ProfessionalId { get; set; }
        public string  FirstName { get; set; }
        public string LastName { get; set; }
        public string  ProfessionName { get; set; }
        public bool TermsandConditions { get; set; } = false;
        public string County { get; set; }
        public bool    Active{ get; set; }
        public string OperationArea { get; set; }
        public string PhoneNumber { get; set; }
        public string  WorkDescription { get; set; }
        public string  Salutation{ get; set; }
        public string  Email { get; set; }
        public string JobNumber { get; set; }
        public string SecondNumber { get; set; }
        public decimal Upvotes { get; set; }
        public decimal Downvotes { get; set; }
        public decimal TotalVotes { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;


    }
}
