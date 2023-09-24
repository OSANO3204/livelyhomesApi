using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.PricingPlansVms
{
   public  class pricingplansvm
    {

        public double PricingAmount { get; set; }
        public string PackageName { get; set; }
        public bool Recommended { get; set; }
        public string PacckageID { get; set; }
        public string CreeatedBy { get; set; }
    }
}
