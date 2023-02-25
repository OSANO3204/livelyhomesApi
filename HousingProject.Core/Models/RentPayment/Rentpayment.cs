using System;
using System.ComponentModel.DataAnnotations;

namespace HousingProject.Core.Models.RentPayment
{
    public   class Rentpayment
    {
        [Key]
        public int RentpaidId { get; set; }

        public string Email { get; set; }

        public int RentPaid { get; set; }

        public int HouseId { get; set; }

        public DateTime Datepaid { get; set; } = DateTime.Now;

        public string Month { get; set; }

        public int TenantId { get; set; }
    }
}
