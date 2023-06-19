using System;
using System.ComponentModel.DataAnnotations;
namespace HousingProject.Core.Models.DelayRequest
{
   public  class RentDelayRequestTable
    {
        [Key]
        public int delay_request_id { get; set; }
       
        public string Requestermail { get; set; }
        public string RequesterNames { get; set; }
        public int HouseId { get; set; }
        public int DoorNumber { get; set; }
        public string RequesterId { get; set; }
        public string AdditionDetails { get; set; }
        public DateTime TenantRentPaymentDate { get; set; }
        public string Status { get; set; }
        public DateTime DateRequested { get; set; } 
        public DateTime RequestDate { get; set; } = DateTime.Now;
    }
}
