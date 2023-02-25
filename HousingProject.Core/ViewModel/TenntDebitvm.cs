using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel
{
 public   class TenntDebitvm
    {
      
        public String Email { get; set; }

        public int TenantId { get; set; }

        public decimal Rentmount { get; set; }

        public string Month { get; set; }
        public string HousedId{get; set;}
    }
}
