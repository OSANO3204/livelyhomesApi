using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.Payment
{
  public   class paymentCodesvm
    {
        public int HouseID { get; set; }
        public string Stk_shortCode { get; set; }
        public string CallbackUrl { get; set; }
    }
}
