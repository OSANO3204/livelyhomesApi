using HousingProject.Architecture.Interfaces.IRenteeServices;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.JobServices.tenantjobs
{
   public class Monthly_Rent_Update:IJob
    {
        private readonly ITenantServices _tenantservices;
        public Monthly_Rent_Update(ITenantServices tenantservices)
        {
            _tenantservices = tenantservices;
        }


        public Task Execute(IJobExecutionContext context)
        {
            _tenantservices.MonthlyRentfn();

            return Task.CompletedTask;
        }
    }
}
