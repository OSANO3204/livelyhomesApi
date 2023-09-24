using HousingProject.Architecture.Data;
using HousingProject.Core.ViewModel.PricingPlansVms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.CRUDServices.PricingPlans
{
    public    class PricingServices
    {
        private readonly HousingProjectContext _context;
        private readonly IServiceScopeFactory _scopefactory;
        public PricingServices(HousingProjectContext context, IServiceScopeFactory scopefactory)
        {
            _context = context;
            _scopefactory = scopefactory;
                
        }

        public async  Task<string>  AddPricingPlans(pricingplansvm vm, string userID)
        {
            try
            {
                using(var scope= _scopefactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider
                                      .GetRequiredService<HousingProjectContext>();
                    var user = await scopedcontext.RegistrationModel.Where(y => y.Id == userID).FirstOrDefaultAsync();

                    if (user == null) return "No user found;";
                    var newPlan = new PricingPlans
                     {
                        PricingAmount= vm.PricingAmount,
                        PackageName=vm.PackageName,
                        Recommended=true,
                        PacckageID= vm.PacckageID,
                        CreeatedBy=user.FirstName + "  "+ user.LasstName

                    };
                    await scopedcontext.AddAsync(newPlan);
                    await scopedcontext.SaveChangesAsync();
                    return "Pricing added successfully ! !";
                }

            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

            

    }
}
