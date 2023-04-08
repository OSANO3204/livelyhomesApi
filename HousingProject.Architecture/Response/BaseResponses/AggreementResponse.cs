using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Response.BaseResponses
{
  public   class AggreementResponse
    {
        public string Code { get;  set; }
        public string Message { get;  set; }
        public object Body { get; set; }
 

        public AggreementResponse( string code ,  string message, object body )
        {
            Code =code ;    
            Message = message;
            Body = body;
         
        }
    }
}
