using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.SuperServices
{
    public   class AdminService: IAdminServices
            {
        private readonly IServiceScopeFactory _scopefactory;
        public AdminService(IServiceScopeFactory scopefactory)
        {
            _scopefactory = scopefactory;
        }


        //public async Task<BaseResponse> User_Activity_Records(string startTime, DateTime startdate, DateTime endtime)
        //{
        //    try
        //    {
        //        using(var scope = _scopefactory.CreateScope())
        //        {
        //            var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        return new BaseResponse { Code = "290", ErrorMessage = ex.Message, Body = ex.StackTrace };
        //    }


        //}


    }
}
