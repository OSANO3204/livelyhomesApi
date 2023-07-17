using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.People.General;
using HousingProject.Core.ViewModel;
using HousingProject.Core.ViewModel.Rentee;
using HousingProject.Core.ViewModel.Rentpayment;
using HousingProject.Infrastructure.Response.payment_ref;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HousingProject.Architecture.Interfaces.IRenteeServices
{
    public  interface ITenantServices
    {
        Task<BaseResponse> Register_Rentee(Rentee_RegistrationViewModel RenteeVm);
        Task<IEnumerable<TenantClass>> GetAllRenteess();
        Task<BaseResponse> GetTenantSummary(int houseId, int tenantId);
        Task<BaseResponse> UpdateRentpaid(int tenantid, float rentadded);
        Task<BaseResponse> GetTenantById(int tenantId);
        Task<BaseResponse> updateTenantRent(int tenantId, RentpaymentViewmodel vm);
        Task<BaseResponse> RentTotal(int tenantid);
        Task<BaseResponse> TenanttotalRent(int tenantId);
        Task<IEnumerable> GetTenantByHouseid(int houseid);
        Task<IEnumerable> rentpaymentList(int tenantId);
        Task<BaseResponse> GetLoggedInTenant();
        Task<BaseResponse> GetLogeedInTenantHouse();
        Task<BaseResponse> SpecificTenantReminderonRentPayment(int tenantid);
        Task<BaseResponse> UpdateRentPayday(DateTime rentpaydate, string email);
        Task AutomtedRentNotiication();
        Task<BaseResponse> ReminderSentEntry(int tenantid);
        Task<BaseResponse> AllRemindersSent(int houseid);
        Task<BaseResponse> PayingRent(int tenantid, decimal rentamount);
        Task<BaseResponse> GetAllTenantPayments(int tenantid);
        Task<BaseResponse> GetHouseUnitBodyById(int houseuintid);
        Task<BaseResponse> RequestRentDelay(string requestdate, string addtionalDetails);
        Task<BaseResponse> GetAll_DelayRequests_By_HouseId(int houseid);
        Task<BaseResponse> GetAll_DelayRequests_By_TenantEmail(string tenantemail);
        Task<BaseResponse> ApproveRequest(int requestid );
        Task<BaseResponse> GetDelayRequetsByHouseIDandStatus(int houseid, string requestStatus);
        Task<BaseResponse> RejectRequest(int requestid);
        Task<string> GetGeneratedref();
        Task MonthlyRentfn();
        Task Reset_Updated_this_month();
        Task<Payments_Reference_Response> Get_Monthly_Rent_Update(int house_id);
        Task<BaseResponse> Vacant_House_update(int house_id, int door_number);
        Task<BaseResponse> Get_All_Occupied_House(int house_id);
        Task<IEnumerable> Get_InActtive_Tenant_By_Houseid(int houseid);

    }
}
