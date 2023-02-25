using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.RentPayment
{
    public class RentDebit
    {

        [Key]
        public int rentID { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public String Email { get; set; }

        public int TenantId { get; set; }

        public decimal Rentmount { get; set; }

        public string Month { get; set; }
        public int HousedId{get; set;}

        public decimal Credit { get; set; }

        public decimal Debit { get; set; }
    }
}
