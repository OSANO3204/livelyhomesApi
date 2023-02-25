using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.WorkIdModel;
using HousingProject.Core.ViewModel.GenerateworkIdVm;
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
        public GenerateIdService(HousingProjectContext context)
        {
            _context = context;
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


            var CurrentNumber = _context.WorkIdModel.OrderByDescending(x=>x.DateCreated).FirstOrDefault();
            var changedid = new WorkIdModel { 
                       
                WorkIdSaved= CurrentNumber.WorkIdSaved + 1 
            
            
            };
        

            _context.Update(changedid);
               await _context.SaveChangesAsync();

            var CurrentNumberupdate = _context.WorkIdModel.OrderByDescending(x=>x.DateCreated).FirstOrDefault();


            var Covertaddedtostring= CurrentNumberupdate.WorkIdSaved.ToString();



            int length = 5;
              const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
              var randomstrings=    new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
              
              
            return new BaseResponse { Code = "200", SuccessMessage = "LH"+ randomstrings+ Covertaddedtostring };

        }
    }
}
