using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.CountiesModel;
using HousingProject.Core.Models.People;
using HousingProject.Infrastructure.ExtraFunctions.IExtraFunctions;
using HousingProject.Infrastructure.ExtraFunctions.vm;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions
{
    public class AddingCountiesCRUD: IextraFunctions
    {
        private readonly IHttpContextAccessor _httpcontextaccessor;
        private readonly HousingProjectContext _context;

        public AddingCountiesCRUD(
            IHttpContextAccessor httpcontextaccessor,
            HousingProjectContext context
            )
        {
            _httpcontextaccessor = httpcontextaccessor;
            _context = context;
        }
        public async Task<RegistrationModel> LoggedInUser()
        {
            var currentuserid =
                _httpcontextaccessor
                    .HttpContext
                    .User
                    .Claims
                    .Where(x => x.Type == "Id")
                    .Select(p => p.Value)
                    .FirstOrDefault();

            var loggedinuser =
                await _context
                    .RegistrationModel
                    .Where(x => x.Id == currentuserid)
                    .FirstOrDefaultAsync();

            return loggedinuser;
        }
        public async Task<BaseResponse> AddCounty(AddCountyvm vm)
        {
            try
            {
                if (vm.CountyName == "")
                {

                    return new BaseResponse { Code = "230", ErrorMessage = "Please add a county name" };
                }

                var user = LoggedInUser().Result;

                if (user == null)
                {

                    return new BaseResponse { Code = "340", ErrorMessage = " Could not find user fast enough to update county " };
                }
                var addcounty = new AddCounty
                {
                    CountyName = vm.CountyName,
                    CreatorID =Convert.ToInt32(user.Id),
                    AddedBy = user.Email
                };

                await _context.AddAsync(addcounty);
                await _context.SaveChangesAsync();
                return new BaseResponse { Code = "200", SuccessMessage = "Congrats!! You added a county successfully!" };
            }
            catch (Exception ex)
            {

                foreach (var error in ex.Message)
                {
                    return new BaseResponse { Code = "230", ErrorMessage = error.ToString() };
                }

                return new BaseResponse { Code = "219", ErrorMessage = "something foreign happened" };

            }
        }
        public async Task<IEnumerable> GetCounties()
        {

            return await _context.AddCounty.ToListAsync();
    }
    }

  
}
