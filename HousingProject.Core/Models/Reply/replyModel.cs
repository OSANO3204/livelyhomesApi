using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.Reply
{
   public  class replyModel
    {
        [Key]
        public int ReplyID { get; set; }
        public string Reply { get; set; }

        public int MessageID { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string ResponseAgent { get; set; }

        public bool Closed { get; set; }
    }
}
