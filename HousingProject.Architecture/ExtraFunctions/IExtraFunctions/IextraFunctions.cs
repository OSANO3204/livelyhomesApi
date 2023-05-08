using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.Professionalsvm;
using HousingProject.Infrastructure.ExtraFunctions.vm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions.IExtraFunctions
{
   public  interface IextraFunctions
    {
        Task<BaseResponse> AddCounty(AddCountyvm vm);
        Task<IEnumerable> GetCounties();
        Task<BaseResponse> AddCountyAreas(AddCountyAreavm vm);
        Task<BaseResponse> GetOperationalareaBycountyid(int countyid);
    }
}
