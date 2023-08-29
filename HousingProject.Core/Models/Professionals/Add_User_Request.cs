using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Professionals
{
 public    class Add_User_Request
    {
        [Key]
        public int request_id { get; set; }
        public string Job_Number { get; set; }
        public string Worker_phone { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public string Worker_Email { get; set; }
        public string Phone_Number { get; set; }
        public string RequesterEmail { get; set; }
        public string Names { get; set; }
        public  bool Is_Closed { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
