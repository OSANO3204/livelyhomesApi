using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.Remindersenttablevm
{
  public   class ReminderSentTablevm
    {
        public int TenantId { get; set; }
        public string TenantNames { get; set; }
        public string TenantEmail { get; set; }
        public string HouseName { get; set; }
        public string ReminderSent { get; set; }
        public int DoorNumber { get; set; }
        public string SendByNames { get; set; }
        public string SentByEmail { get; set; }
    }
}
