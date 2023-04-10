using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Interfaces.ITenantStatementServices
{
    public interface ITenantStatementServices
    {
        Task<BaseResponse> Rentpayments(TenntDebitvm vm);


    }
}
