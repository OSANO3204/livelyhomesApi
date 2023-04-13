using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel
{
  public   class message_replybody
    {
        public string Subject { get; set; }
        public string  Message { get; set; }
        public string replymessage { get; set; }
        public string sendermail { get; set; }

        public string Receivername { get; set; }
        public string AgentName { get; set; }
        public  string CompanyPhone { get; set; }
        public DateTime SentOn { get; set; } 

    }
}
