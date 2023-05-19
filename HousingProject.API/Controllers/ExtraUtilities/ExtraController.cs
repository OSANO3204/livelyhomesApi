using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.Professionalsvm;
using HousingProject.Infrastructure.ExtraFunctions.IExtraFunctions;
using HousingProject.Infrastructure.ExtraFunctions.vm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;


namespace HousingProject.API.Controllers.ExtraUtilities
{
    [Route("api/[controller]", Name = "Building_Apartment")]
    [ApiController]

    public class ExtraController : IextraFunctions
    {
        public readonly IextraFunctions _iextraFunctions;
        public ExtraController(IextraFunctions iextraFunctions)
        {
            _iextraFunctions = iextraFunctions;
        }

        [Authorize]
        [Route("AddCounty")]
        [HttpPost]
        public async Task<BaseResponse> AddCounty(AddCountyvm vm)
        {
            return await _iextraFunctions.AddCounty(vm);
        }

        [Authorize]
        [Route("GetAllCounties")]
        [HttpGet]
        public async Task<IEnumerable> GetCounties()
        {
            return await _iextraFunctions.GetCounties();
        }

        [Authorize]
        [Route("GetOperationalareabyCountyid")]
        [HttpPost]
        public async Task<BaseResponse> GetOperationalareaBycountyid(int countyid)
        {
            return await _iextraFunctions.GetOperationalareaBycountyid(countyid);

        }

        [Authorize]
        [Route("AddCountyoperationalareas")]
        [HttpPost]
        public async Task<BaseResponse> AddCountyAreas(AddCountyAreavm vm)
        {
            return await _iextraFunctions.AddCountyAreas(vm);
        }
        


        
    }
}
