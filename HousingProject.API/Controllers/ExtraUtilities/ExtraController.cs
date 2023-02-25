using HousingProject.Architecture.Response.Base;
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

    public class ExtraController: IextraFunctions
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
            return await  _iextraFunctions.GetCounties();
        }
    }
}
