using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.WorkIdModel;
using HousingProject.Core.ViewModel.GenerateworkIdVm;
using HousingProject.Infrastructure.Interfaces.IUserExtraServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions.GenerateWorkId
{

   
   public  class GenerateIdService: IGenerateIdService
    {

        private readonly HousingProjectContext _context;
        private static Random random = new Random();
        private readonly IUserExtraServices _iuser_services;
        public GenerateIdService(HousingProjectContext context, IUserExtraServices iuser_services)
        {
            _context = context;
            _iuser_services = iuser_services;

        }

        public async Task<BaseResponse> GenerateWorlkIdFn(GenerateworkIdVm  vm)
        {
            try
            {
                var newwork_id = new WorkIdModel 
                {
                    WorkIdSaved = vm.WorkIdSaved 
                };
                await _context.AddAsync(newwork_id);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return new BaseResponse { Code = "356", ErrorMessage = ex.ToString() };
            }

            return new BaseResponse { Code = "457", ErrorMessage = "something else happened"};
        }

    
        public async Task<BaseResponse> GenerateWorkId()
        {


            var CurrentNumber = _iuser_services.GenerateReferenceNumber(5);         
              
            return new BaseResponse { Code = "200", SuccessMessage = "LH"+ CurrentNumber };

        }
    }
}
