using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.Rentpayment
{
    public class update_rent_table
    {
        public int Tenantid { get; set; }    
        public double RentAmount { get; set; }
        public double Balance { get; set; }
        public double Paid { get; set; }
        public string PhoneNumber { get; set; }
        public string Internal_ReferenceNumber { get; set; }
        public string Provider_Reference { get; set; }
   

    }
}
