using HousingProject.Architecture.Data;
using HousingProject.Core.Models.People;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Back
{
    public  class BackMethods
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HousingProjectContext _context;
        public BackMethods(IHttpContextAccessor httpContextAccessor, HttpContextAccessor httpContextAccessor1, HousingProjectContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<RegistrationModel> LoggedInUser()
        {


            var currentuserid = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == "Id").Select(p => p.Value).FirstOrDefault();
            var loggedinuser = await _context.RegistrationModel.Where(x => x.Id == currentuserid).FirstOrDefaultAsync();

            return loggedinuser;


        }




    }
}
