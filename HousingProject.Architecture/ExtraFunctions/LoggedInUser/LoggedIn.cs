using HousingProject.Architecture.Data;
using HousingProject.Core.Models.People;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions.LoggedInUser
{
    public   class LoggedIn: ILoggedIn
    {
        
        private readonly IHttpContextAccessor _httpcontextaccessor;
        private readonly HousingProjectContext _context;
        public LoggedIn(
            IHttpContextAccessor httpcontextaccessor,
            HousingProjectContext context

            )
        {
            _httpcontextaccessor = httpcontextaccessor;
            _context = context;
        }

        public async Task<RegistrationModel> LoggedInUser()
        {
            try
            {
                var currentuserid = _httpcontextaccessor.HttpContext.User.Claims.Where(x => x.Type == "Id").Select(p => p.Value).FirstOrDefault();

                var loggedinuser =await _context.RegistrationModel.Where(x => x.Id == currentuserid).FirstOrDefaultAsync();

                return loggedinuser;
            }
            catch(Exception ex)
            {

                foreach(var error in ex.Message)
                {

                    return new RegistrationModel { Salutation = error.ToString() };
                }
            }

            return new RegistrationModel { Salutation = "Nothing to show here" };
           
        }
    }
}
