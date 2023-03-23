using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel
{
 public    class EmailNotificationOnRentPayment
    {

        public string ToEmail { get; set; }
        public string UserName { get; set; }
        public string PayLoad { get; set; }

        public string Message { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime sentDate { get; set; } 

        public double RenTAmount { get; set; }
    }
}
