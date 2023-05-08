using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel
{
  public  class AutomaticMessaging
    {

        public string ToEmail { get; set; }
        public string TenantNmes { get; set; }
        public string Meessage { get; set; }
        public DateTime SentDate { get; set; } = DateTime.Now;
    }
}
