using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.GenerateworkIdVm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions.GenerateWorkId
{
   public  interface IGenerateIdService
    {
        Task<BaseResponse> GenerateWorlkIdFn(GenerateworkIdVm vm);
        Task<BaseResponse> GenerateWorkId();
    }
}
