using HousingProject.Architecture.Interfaces.IEmail;
using HousingProject.Architecture.Interfaces.IRenteeServices;
using HousingProject.Core.ViewModel;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.JobServices
{

   public  class automatedMail:IJob
    {
        private readonly ITenantServices _tenantservices;
        public automatedMail(ITenantServices tenantservices)
        {
            _tenantservices = tenantservices;
        }

        public  Task Execute(IJobExecutionContext context)
        {
            _tenantservices.AutomtedRentNotiication();

            return Task.CompletedTask;
        }
    }
}
