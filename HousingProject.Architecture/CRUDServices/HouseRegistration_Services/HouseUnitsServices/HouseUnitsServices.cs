using HousingProject.Architecture.Data;
using HousingProject.Architecture.IHouseRegistration_Services;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Houses.HouseUnitRegistration;
using HousingProject.Core.ViewModel.HouseUnitRegistrationvm;
using HousingProject.Infrastructure.ExtraFunctions;
using HousingProject.Infrastructure.Interfaces.IHouseRegistration_Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.CRUDServices.HouseRegistration_Services.HouseUnitsServices
{
    public class HouseUnitsServices: IHouseUnits
    {
        private readonly HousingProjectContext _context;
        private readonly IverificationGenerator _iverificationGenerator;
        public HouseUnitsServices(
            IverificationGenerator iverificationGenerator,
            HousingProjectContext context
            )
        {
            _iverificationGenerator = iverificationGenerator;
            _context = context;
        }

        public async Task<BaseResponse> GenarateString()
        {


            Random rand = new();


            int stringlen = rand.Next(4, 10);
            int randValue;
            string str = "";
            char letter;
            for (int i = 0; i < stringlen; i++)
            { randValue = rand.Next(0, 26);
                var randomvalue = randValue + 65;
                var ranomvalue1 = randomvalue++;
                letter = Convert.ToChar(ranomvalue1);


                str = str + letter;
            }
            return new BaseResponse { Code = "200", SuccessMessage = str };
         }



        public async Task<BaseResponse> RegisterHouseUnit(HouseUnitRegistrationvm vm)
        {
            try
            {
           
                var generatedstring = GenarateString().Result;

                var storednumber = await _context.GeneratedIdHolder.Select(x => x.GeneratorHolder).FirstOrDefaultAsync();


                var thestorednumber = storednumber;
                var newnumbers = thestorednumber + 1;

                var newnumber = new GeneratedIdHolder
                                {
                                    GeneratorHolder = newnumbers
                                };

                 _context.Update(newnumber);
                await _context.SaveChangesAsync();





                var generatedtoken = "LHUID" + generatedstring.SuccessMessage +"_" + newnumbers;
              
                var checktoken = await _context.HouseUnit.Where(x => x.GeneratedId == generatedtoken).FirstOrDefaultAsync();




                if (checktoken != null)
                {


                    return new BaseResponse { Code = "458", ErrorMessage = "an error cooured, kindly try again" };


                }
                var houseunit = new HouseUnit
                    {
                        HouseID = vm.HouseID,
                        HouseUnitNumber = vm.HouseUnitNumber,
                        Occupied = vm.Occupied,
                        Vacant = vm.Vacant,
                        HouseUnitFloor = vm.HouseUnitFloor,
                        GeneratedId = generatedtoken

                    };

                await _context.AddAsync(houseunit);
                await _context.SaveChangesAsync();

                return new BaseResponse { Code = "200", SuccessMessage = "House unit registered successfully", Body=houseunit };

            }

            catch (Exception ex)
            {
                return new BaseResponse { Code = "473", ErrorMessage = ex.Message };
            }
        }

       
    }

}
    

