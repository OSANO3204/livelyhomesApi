using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.Landlord;
using HousingProject.Core.Models.People.Landlord;
using System.Threading.Tasks;
using HousingProject.Architecture.Interfaces.ILandlordModel;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace HousingProject.Architecture.Services.Landlord
{
    public class LanlordServices : ILandlordServices
    {
        private readonly HousingProjectContext _context;
        public LanlordServices(HousingProjectContext context)
        {
            _context = context;
        }




        public async Task<BaseResponse> LandlongHouse_Registration(LandlordHouse_RegistrationVm vm)
        {

            var LandlordHoyseRegistration = new Landlordmodel
            {


                FirstName = vm.FirstName,
                LasstName = vm.LasstName,
                IdNumber = vm.IdNumber,
                BirthDate = vm.BirthDate,
                Email = vm.Email,
                UserName = vm.UserName,
                HouseLocation = vm.HouseLocation,
                RentCollection_Date = vm.RentCollection_Date,
                Paybill_Number = vm.Paybill_Number,
                Till_Number = vm.Till_Number

            };

            if (vm.FirstName == "")
            {

                return (new BaseResponse { Code = "101", ErrorMessage = "Firstname can't be empty" });

            }
            if (vm.LasstName == "")
            {

                return (new BaseResponse { Code = "102", ErrorMessage = "Lastname can't be empty" });

            }

            await _context.Landlordmodel.AddAsync(LandlordHoyseRegistration);
            await _context.SaveChangesAsync();
            return (new BaseResponse { Code = "200", SuccessMessage = "House Registered Successfully!" });



        }




        public async Task<IEnumerable> GetLandlordRegisteredHouses()
        {

            return await _context.Landlordmodel.ToListAsync();
        }
    }
}
