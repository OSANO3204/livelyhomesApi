using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.RentMonthly
{
 public    class Rent_Monthly_Update
    {
        [Key]
        public int Monthlyrentid { get; set; }
        public int Tenantid { get; set; }
        public string Tenantnames { get; set; }
        public string Tenant_Email { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public double RentAmount { get; set; }
        public double Balance { get; set; }
        public string HouseName { get; set; }
        public double Paid { get; set; }
        public string PhoneNumber { get; set; }
        public string Internal_ReferenceNumber { get; set; }
        public string Provider_Reference { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public bool Updated_This_Month { get; set; }
        public int House_ID { get; set; }
        public bool ReceiptSent { get; set; }

    }
}
