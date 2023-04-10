using HousingProject.Architecture.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Interfaces.IUserExtraServices
{
   public  interface IUserExtraServices
    {
        Task<BaseResponse> GetAllMessages();
        Task<BaseResponse> GeetMessageById(int messageid);
    }
}
