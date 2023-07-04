using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Response
{
  public   class professional_profile_Response
    {
        public string Code { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public int Totals { get; set; }
        public object Body { get; set; }

        public double Rating { get; set; }

        public DateTime Start_operation_Time { get; set; }
        public DateTime End_operation_Time { get; set; }

        public object RequestsBody { get; set; }

        public string services_body { get; set; }
        public bool isTrue { get; set; }
    }
}
