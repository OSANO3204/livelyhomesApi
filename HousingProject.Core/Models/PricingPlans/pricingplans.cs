using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.CRUDServices.PricingPlans
{
   public  class PricingPlans
    {
        [Key]
        public int pricingplansID { get; set; }
        public double PricingAmount { get; set; }
        public string PackageName { get; set; }
        public bool Recommended { get; set; }
        public string PacckageID { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreeatedBy { get; set; }
    }
}
