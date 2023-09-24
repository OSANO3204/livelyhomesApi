using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Email
{
    public class email_to_technician
    {
        public string ToEmail { get; set; }
        public string UserName { get; set; }
        public string PayLoad { get; set; }
        public string TechnicianNames { get; set; }
    }
}
