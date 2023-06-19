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
        public string Year { get; set; }
        public double RentAmount { get; set; }
        public double Balance { get; set; }
        public string HouseName { get; set; }
        public DateTime MyProperty { get; set; } = DateTime.Now;

    }
}
