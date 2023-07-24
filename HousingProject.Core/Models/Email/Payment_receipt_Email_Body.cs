using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Email
{
  public  class Payment_receipt_Email_Body
    {
        public string ToEmail { get; set; }
        public string UserName { get; set; }
        public string PayLoad { get; set; }
        public string TenantNames { get; set; }
        public double Balance { get; set; }
        public  double Total_Paid { get; set; }
        public string Tenant_Phone { get; set; }
        public string HouseName { get; set; }
        public int DoorNumber { get; set; }
        public string HouseLocation { get; set; }
        public string Caretaker_Phone { get; set; }
        public double Rent_Amount { get; set; }
    }
}
