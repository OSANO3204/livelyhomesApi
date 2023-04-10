using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Infrastructure.Interfaces.IUserExtraServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.CRUDServices.UsersExtra
{
     public   class UserExtraServices: IUserExtraServices
    {

        private readonly HousingProjectContext _context;
        private readonly IServiceScopeFactory _servicefactory;


        public UserExtraServices(HousingProjectContext context, IServiceScopeFactory servicefactory)
                {

                 _context = context;
                 _servicefactory = servicefactory;
        }



        public async Task<BaseResponse> GetAllMessages()
        {

            using (var scope = _servicefactory.CreateScope())
            {
                try
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    var allmessages = scopedcontext.ContactUs.OrderByDescending(d=>d.DateCreated).ToList();

                    if (allmessages == null)
                    {

                        return new BaseResponse { Code = "140", ErrorMessage = "No messages found " };
                    }
                    var totalmessages = allmessages.Count();
                    return new BaseResponse { Code = "200", SuccessMessage = "Queried successfully", Body = allmessages ,Totals= totalmessages, isTrue=true};
                }
                catch(Exception ex)
                {
                    return new BaseResponse { Code = "120", ErrorMessage = ex.Message };
                }

            }
        }

        public async Task<BaseResponse> GeetMessageById(int messageid)
        {
            try
            {
                using (var scope = _servicefactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    var messageexists =  scopedcontext.ContactUs.Where(m => m.ContacusId == messageid).FirstOrDefault();

                    if (messageexists == null)
                    {

                        return new BaseResponse { Code = "160", ErrorMessage = "message does not exist" };
                    }

                    return new BaseResponse { Code = "200", SuccessMessage = "Queried successfully", Body = messageexists };
                }
            }
            catch (Exception ex)
            {

                return new BaseResponse { Code = "180", ErrorMessage = ex.Message };
            }
        }

    }

}
