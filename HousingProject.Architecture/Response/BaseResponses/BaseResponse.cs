using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Architecture.Response.Base
{
   public  class BaseResponse
    {
        public string Code { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; } 

        public int Totals { get; set; }
        public object  Body{ get; set; }

        public bool isTrue { get; set; }
    }
}
