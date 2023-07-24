using System;
using System.ComponentModel.DataAnnotations;

namespace HousingProject.Core.Models.RentPayment
{
    public  class PayRent
    {
       [Key]
        public int Payrentid { get; set; }
        public int TenantId { get; set; }
        public decimal RentAmount { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public bool Completed { get; set; }
        public string InternalReference { get; set; }
        public string ProviderReference { get; set; }
        public int HouseID { get; set; }
        public string Merchant_Request_ID { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool ReceiptSent { get; set; }
    }
}
