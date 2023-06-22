using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Response
{
  public   class Housing_Profile_Response
    {
        public string Code { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public int Occupied_Units { get; set; }
        public int Un_Occupied_Units { get; set; }
        public int Total_Units { get; set; }
        public float Total_Rent { get; set; }
        public float Total_Expected_Service { get; set; }
        public object Body { get; set; }
        public bool isTrue { get; set; }
        public float Total_Amounts { get; set; }

        public int Total_Tenants { get; set; }
    }
}
