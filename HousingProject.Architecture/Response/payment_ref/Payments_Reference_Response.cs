using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Response.payment_ref
{
   public  class Payments_Reference_Response
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public double Balance_left { get; set; }
        public double Total_paid { get; set; }
        public object Body { get; set; }
        public string Current_month { get; set; }
        public int vacant_houses { get; set; }

    }
}
