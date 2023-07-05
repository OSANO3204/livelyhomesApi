using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.RentPayment
{
  public   class Stk_Push_Response_Body
    {
        [Key]
        public int stk_body_id { get; set; }

        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string CustomerMessage { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

    }
}
