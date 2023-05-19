
using HousingProject.Architecture.Data;
using HousingProject.Architecture.Interfaces.IEmail;
using HousingProject.Architecture.Interfaces.IRenteeServices;
using HousingProject.Core.Models.Email;
using Quartz;
using System.Collections;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.JobServices
{
    public class Emailjob : IJob
    {
        private readonly IEmailServices _iemailservices;
        public readonly ITenantServices _tenantServices;
        private readonly HousingProjectContext _context;
        public Emailjob(
                        IEmailServices iemailservices,
                        ITenantServices tenantServices,
                        HousingProjectContext context
                        )
                        {
                            _iemailservices = iemailservices;
                            _tenantServices = tenantServices;
                            _context = context;
                        }


        public async Task<IEnumerable> getalltenants()
        {
            return await _tenantServices.GetAllRenteess();
        }

        public Task Execute(IJobExecutionContext context)
        {

            var sendbody = new UserEmailOptions
            {
                UserName = "A reminder",
                PayLoad = "To remind you to pay your rent ",
                ToEmail = "osano3204@gmail.com"
            };           
            //_iemailservices.sendEmailOnHouseRegistration(sendbody);
                return Task.CompletedTask;

            }
        }
    }

