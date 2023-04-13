using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Response.ReplyResponse
{
 public    class messagereplyresponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public object Replybody{ get; set; }
        

        public messagereplyresponse( string code, string reply, object replybody)
        {
            Code = code;
            Message = reply;
            Replybody = replybody;
            
        }
    }
}
