using HousingProject.Architecture.Interfaces.IRenteeServices;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.JobServices.tenantjobs
{
  public   class Back_monthly_update:IJob
    {
        private readonly ITenantServices _itenant_services;

        public Back_monthly_update(ITenantServices itenant_services)
        {
            _itenant_services = itenant_services;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _itenant_services.Reset_Updated_this_month();

            return Task.CompletedTask;
        }
    }
}
