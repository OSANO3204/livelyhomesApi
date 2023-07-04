using HousingProject.Architecture.Data;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
using HousingProject.Infrastructure.Interfaces.ITenantStatementServices;

namespace HousingProject.Infrastructure.CRUDServices.Payments.Rent
{
    public class TenantStatementServices: ITenantStatementServices
    {

        private readonly ILoggedIn _loggedIn;
        private HousingProjectContext _context;

        public TenantStatementServices(ILoggedIn loggedIn, HousingProjectContext context)
        {
            _loggedIn = loggedIn;
            _context = context;
        }

        //public async Task<BaseResponse> Rentpayments(TenntDebitvm vm)
       // {
           
        //    try
        //    {
        //         var user = _loggedIn.LoggedInUser().Result;
        //    var tenant = await _context.TenantClass.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
        //        var rentdebit = new RentDebit
        //        {
        //            Email = vm.Email,
        //            TenantId = tenant.RenteeId,
        //            Rentmount = vm.Rentmount,
        //            Month = vm.Month,
        //            HousedId = tenant.HouseiD,
        //            Credit = -1 * vm.Rentmount,
        //            Debit = vm.Rentmount

        //        };
        //        await _context.AddAsync(rentdebit);
        //        await _context.SaveChangesAsync();
        //        return new BaseResponse { Code = "200", SuccessMessage = "Retails updated successfully", Body = rentdebit };
        //    } 

        //    catch(Exception ex)
        //    {
        //        return new BaseResponse { Code = "230", ErrorMessage = ex.Message };
        //    }
        //}

    }

}
