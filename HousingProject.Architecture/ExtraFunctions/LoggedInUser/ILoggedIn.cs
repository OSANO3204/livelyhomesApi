using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.People;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions.LoggedInUser
{
    public   interface ILoggedIn
    {
        Task<RegistrationModel> LoggedInUser();
  


    }
}
