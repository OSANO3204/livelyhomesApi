using HousingProject.Infrastructure.CRUDServices.MainPaymentServices;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.JobServices.Payment_Receipts
{
  public   class Payment_Receipt_Job:IJob
    {
        private readonly IpaymentServices _paymentservices;
        public Payment_Receipt_Job(IpaymentServices paymentservices)
        {
            _paymentservices = paymentservices;
        }

        public   Task Execute(IJobExecutionContext context)
        {
            _paymentservices.SendReceipts();

            return Task.CompletedTask;
        }
    }
}
