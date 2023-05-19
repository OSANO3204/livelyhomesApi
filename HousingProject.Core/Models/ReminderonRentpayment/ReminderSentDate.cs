using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.ReminderonRentpayment
{
  public   class ReminderSentDate
    {
        [Key]
        public int ReminderTableId { get; set; }
        public int TenantId { get; set; }
        public string TenantNames { get; set; }
        public string TenantEmail { get; set; }
        public string HouseName { get; set; }
        public string ReminderSent { get; set; }

        public int DoorNumber { get; set; }

        public string SendByNames { get; set; }
        public string SentByEmail { get; set; }

        public int HouseId { get; set; }
        public DateTime DateSent { get; set; } = DateTime.Now;

    }
}
