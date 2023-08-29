using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.ActivityTracker
{
   public  class activityTracker
    {
        [Key]
        public int activityID { get; set; }
        public string ActivityKey { get; set; }
        public string UserEEmail { get; set; }
        public string UserNames { get; set; }
        public string PageName { get; set; }
        public int TimeSpent_In_Minutes { get; set; }
        public DateTime DateUpdate { get; set; } = DateTime.Now;
    }
}
