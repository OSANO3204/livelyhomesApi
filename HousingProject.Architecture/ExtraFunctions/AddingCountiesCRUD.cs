using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.CountiesModel;
using HousingProject.Core.Models.People;
using HousingProject.Core.ViewModel.Professionalsvm;
using HousingProject.Infrastructure.ExtraFunctions.IExtraFunctions;
using HousingProject.Infrastructure.ExtraFunctions.vm;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions
{
    public class AddingCountiesCRUD : IextraFunctions
    {
        private readonly IHttpContextAccessor _httpcontextaccessor;
        private readonly HousingProjectContext _context;
        private readonly IServiceScopeFactory _serviceScope;
        public AddingCountiesCRUD(
            IHttpContextAccessor httpcontextaccessor,
            HousingProjectContext context,
            IServiceScopeFactory serviceScope
            )
        {
            _httpcontextaccessor = httpcontextaccessor;
            _context = context;
            _serviceScope = serviceScope;
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

                var countyexists = await _context.AddCounty.Where(c => c.CountyName == vm.CountyName).FirstOrDefaultAsync();

                if (countyexists != null)
                {

                    return new BaseResponse { Code = "120", ErrorMessage = "County already added" };
                }
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
                    CreatorID = user.Id,
                    AddedBy = user.Email
                };

                var getcounty = addcounty;

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
        public async Task<BaseResponse> AddCountyAreas(AddCountyAreavm vm)
        {
            try
            {

                using (var scope = _serviceScope.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();


                    var operationalareaexists = await scopedcontext.AddCountyArea.Where(a => a.CountyArea == vm.CountyArea).FirstOrDefaultAsync();


                    if (operationalareaexists != null)
                    {

                        return new BaseResponse { Code = "170", ErrorMessage = "Operational area already exists" };
                    }

                    var countyexists = await scopedcontext.AddCounty.Where(c => c.CountyId == vm.CountyId).FirstOrDefaultAsync();

                    if (countyexists == null)
                    {

                        return new BaseResponse { Code = "140", ErrorMessage = "County does not exist" };
                    }

                    //create new county area

                    var newcountyarea = new AddCountyArea
                    {

                        CountyArea = vm.CountyArea,
                        CountyId = countyexists.CountyId

                    };

                    await scopedcontext.AddAsync(newcountyarea);
                    await scopedcontext.SaveChangesAsync();


                    return new BaseResponse { Code = "200", SuccessMessage = $"You have successfully updated county {countyexists.CountyName} with  operational area {vm.CountyArea} " };

                }

            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "170", ErrorMessage = ex.Message };
            }
        }

        public async Task<BaseResponse> GetOperationalareaBycountyid(int countyid)
        {
            try
            {
                using (var scope = _serviceScope.CreateScope())
                {
                   var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var operationalareaxexists = await scopedcontext.AddCountyArea.Where(O => O.CountyId == countyid).ToListAsync();

                    if (operationalareaxexists == null)
                    {
                        return new BaseResponse { Code = "140", ErrorMessage = "No operational areas found" };
                    }

                    return new BaseResponse { Code = "200", SuccessMessage = "Successfully queried opertional area(s)", Body = operationalareaxexists };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "170", ErrorMessage = ex.Message };

            }


        }


    }
}
