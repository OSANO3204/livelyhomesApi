using HousingProject.Architecture.Models.People;
using HousingProject.Core.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.Models.People.Landlord
{
   public  class Landlordmodel:BaseClass
    {
        [Key]
        public int LandlordId { get; set; }

        public string HouseLocation{ get; set; }

        public string HouseId { get; set; }

        public DateTime RentCollection_Date { get; set; }

        public string LondLord_HouseId { get; set; }

        public string Paybill_Number { get; set; }

        public string Till_Number { get; set; }

        public List<TenantSummary> Tenant { get; set; }




    }
}
