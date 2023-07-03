using HousingProject.Architecture.Interfaces.IRenteeServices;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.ViewModel.Rentee;
using HousingProject.Core.ViewModel.Rentpayment;
using HousingProject.Infrastructure.Interfaces.ITenantStatementServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace HousingProject.API.Controllers.Rentee
{


    [Route("api/[controller]", Name = "Rentee")]
    [ApiController]
    public class TenantController
    {

        private readonly ITenantStatementServices _tenantStatementServices;
        private readonly ITenantServices _irenteeServices;
        public TenantController(ITenantServices irenteeServices, ITenantStatementServices tenantStatementServices)
        {
            _irenteeServices = irenteeServices;
            _tenantStatementServices = tenantStatementServices;
        }

        [Authorize]
        [Route("Register_Rentee")]
        [HttpPost]
        public async Task<BaseResponse> Register_Rentee(Rentee_RegistrationViewModel RenteeVm)
        {
            return await _irenteeServices.Register_Rentee(RenteeVm);
        }

        [Authorize]
        [Route("Tenanttotalrent")]
        [HttpPost]
        public async Task<BaseResponse> TenanttotalRent(int tenantId)
        {
            return await _irenteeServices.TenanttotalRent(tenantId);
        }


        [Authorize]
        [Route("updatetRent")]
        [HttpPost]
        public async Task<BaseResponse> updateTenantRent(int tenantId, RentpaymentViewmodel vm)
        {
            return await _irenteeServices.updateTenantRent(tenantId, vm);
        }

        [Authorize]
        [Route("GetAllRentees")]
        [HttpGet]
        public async Task<IEnumerable> GetAllRenteess()
        {
            return await _irenteeServices.GetAllRenteess();
        }

        [Authorize]
        [Route("GetTenantStatements")]
        [HttpPost]
        public async Task<BaseResponse> GetTenantSummary(int houseId, int tenantId)
        {

            return await _irenteeServices.GetTenantSummary(houseId, tenantId);
        }

        [Authorize]
        [Route("agenttotalrent")]
        [HttpPost]
        public async Task<BaseResponse> RentTotal(int tenantid)
        {
            return await _irenteeServices.RentTotal(tenantid);
        }

        [Authorize]
        [Route("Gettenantpaymentstatements")]
        [HttpPost]
        public async Task<IEnumerable> rentpaymentList(int tenantId)
        {
            return await _irenteeServices.rentpaymentList(tenantId);
        }

        [Authorize]
        [Route("updateRentDetails")]
        [HttpPost]
        public async Task<BaseResponse> UpdateRentpaid(int tenantid, float rentadded)
        {
            return await _irenteeServices.UpdateRentpaid(tenantid, rentadded);
        }
        [Authorize]
        [Route("GetTeanntById")]
        [HttpPost]
        public async Task<BaseResponse> GetTenantById(int tenantId)
        {
            return await _irenteeServices.GetTenantById(tenantId);
        }

        [Authorize]
        [Route("GettenantsbyHouseId")]
        [HttpPost]
        public async Task<IEnumerable> GetTenantByHouseid(int houseid)
        {

            return await _irenteeServices.GetTenantByHouseid(houseid);
        }

        [Authorize]
        [Route("GetTenantloggedin")]
        [HttpGet]
        public async Task<BaseResponse> GetLoggedInTenant()
        {

            return await _irenteeServices.GetLoggedInTenant();
        }

        [Authorize]
        [Route("GetLoggedInTenantHouse")]
        [HttpGet]
        public async Task<BaseResponse> GetLogeedInTenantHouse()
        {

            return await _irenteeServices.GetLogeedInTenantHouse();
        }


        //[Authorize]
        //[Route("TenantRentPayment")]
        //[HttpPost]
        //public async Task<BaseResponse> Rentpayments(TenntDebitvm vm)
        //{

        //    return await _tenantStatementServices.Rentpayments(vm);
        //}


        [Authorize]
        [Route("Tenantreminderonrentpayment")]
        [HttpPost]
        public async Task<BaseResponse> SpecificTenantReminderonRentPayment(int tenantid)
        {

            return await _irenteeServices.SpecificTenantReminderonRentPayment(tenantid);
        }

        [Authorize]
        [Route("UpdateTenantrentpayday")]
        [HttpPost]
        public async Task<BaseResponse> UpdateRentPayday(DateTime rentpaydate, string email)
        {

            return await _irenteeServices.UpdateRentPayday(rentpaydate, email);
        }

        [Authorize]
        [Route("allsentreminders")]
        [HttpPost]
        public async Task<BaseResponse> AllRemindersSent(int houseid)
        {
            return await _irenteeServices.AllRemindersSent(houseid);

        }
        [Authorize]
        [Route("PayingRent")]
        [HttpPost]
        public async Task<BaseResponse> PayingRent(int tenantid, decimal rentamount)
        {
            return await _irenteeServices.PayingRent(tenantid, rentamount);

        }

        [Authorize]
        [Route("GettingAllTenantPayment")]
        [HttpPost]
        public async Task<BaseResponse> GetAllTenantPayments(int tenantid)
        {

            return await _irenteeServices.GetAllTenantPayments(tenantid);
            return await _irenteeServices.GetAllTenantPayments(tenantid);
        }

        [Authorize]
        [Route("GetHouseUnitByiD")]
        [HttpPost]
        public async Task<BaseResponse> GetHouseUnitBodyById(int houseuintid)
        {
            return await _irenteeServices.GetHouseUnitBodyById(houseuintid);
        }

        [Authorize]
        [Route("RequestRentDelay")]
        [HttpPost]
        public async Task<BaseResponse> RequestRentDelay(string requestdate, string addtionalDetails)
        {

            return await _irenteeServices.RequestRentDelay(requestdate, addtionalDetails);
        }


        [Authorize]
        [Route("GetDelaysbyHouseId")]
        [HttpPost]
        public async Task<BaseResponse> GetAll_DelayRequests_By_HouseId(int houseid)
        {
            return await _irenteeServices.GetAll_DelayRequests_By_HouseId(houseid);
        }

        [Authorize]
        [Route("GetDelaysByTenantEmail")]
        [HttpPost]
        public async Task<BaseResponse> GetAll_DelayRequests_By_TenantEmail(string tenantemail)
        {
            return await _irenteeServices.GetAll_DelayRequests_By_TenantEmail(tenantemail);
        }

        [Authorize]
        [Route("Approve_Request")]
        [HttpPost]
        public async Task<BaseResponse> ApproveRequest(int requestid)
        {
            return await _irenteeServices.ApproveRequest(requestid);
        }

        [Authorize]
        [Route("GetAllRequestsByStatusAndHouseId")]
        [HttpPost]
        public async Task<BaseResponse> GetDelayRequetsByHouseIDandStatus(int houseid, string requestStatus)
        {
            return await _irenteeServices.GetDelayRequetsByHouseIDandStatus(houseid, requestStatus);
        }

        [Authorize]
        [Route("Reject_Rent_ Delay_Request")]
        [HttpPost]
        public async Task<BaseResponse> RejectRequest(int requestid)
        {
            return await _irenteeServices.RejectRequest(requestid);
        }





    }
}
