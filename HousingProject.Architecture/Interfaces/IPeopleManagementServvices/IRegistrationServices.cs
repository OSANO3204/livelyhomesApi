using HousingProject.Architecture.Models.People;
using HousingProject.Architecture.Response.Base;
using HousingProject.Architecture.ViewModel.People;
using HousingProject.Core.ViewModel.People.GeneralRegistration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Architecture.IPeopleManagementServvices
{
   public  interface IRegistrationServices
    {
 

        Task<IEnumerable> GetAllUsers();
        Task<BaseResponse> GetUserByUsername(string username);
        Task<BaseResponse> AsigRole(AsignRoleviewModel vm);
        //identity registration
        Task<BaseResponse> UserRegistration(RegisterViewModel registervm);
        Task<BaseResponse> AccountVerification(string verificationtoken);
        Task<BaseResponse> RomveRole(AsignRoleviewModel vm);
    }
}
