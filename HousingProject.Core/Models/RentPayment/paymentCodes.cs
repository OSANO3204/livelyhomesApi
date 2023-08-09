using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.RentPayment
{
  public   class paymentCodes
    {
        [Key]
        public int paymentCodeid { get; set; }
        public int HouseID { get; set; }
        public string HouseName { get; set; }
        public string Stk_shortCode { get; set; }
        public string  CallbackUrl { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public bool UseDefault { get; set; } = true;
        public bool Setup_Done { get; set; }
    }
}
