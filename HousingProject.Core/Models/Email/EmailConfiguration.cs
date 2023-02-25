using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Email
{
    public class EmailConfiguration
    {
        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
   
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }

    }
}
