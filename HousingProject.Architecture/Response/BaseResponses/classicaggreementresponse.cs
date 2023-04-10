using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Response.BaseResponses
{
   public class classicaggreementresponse
    {

        public string Code { get; set; }
        public string Message { get; set; }
        public string HouseName { get; set; }
        public object Body { get; set; }
       


        public classicaggreementresponse(string code, string message, string housename,object body)
        {
            Code = code;
            Message = message;
            Body = body;
            HouseName = housename;

        }
    }
}
