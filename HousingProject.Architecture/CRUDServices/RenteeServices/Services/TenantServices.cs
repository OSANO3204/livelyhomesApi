using HousingProject.Architecture.Data;
using HousingProject.Architecture.Interfaces.IEmail;
using HousingProject.Architecture.Interfaces.IRenteeServices;
using HousingProject.Architecture.IPeopleManagementServvices;
using HousingProject.Architecture.Response.Base;
using HousingProject.Architecture.ViewModel.People;
using HousingProject.Core.Models.DelayRequest;
using HousingProject.Core.Models.Email;
using HousingProject.Core.Models.General;
using HousingProject.Core.Models.People;
using HousingProject.Core.Models.People.General;
using HousingProject.Core.Models.ReminderonRentpayment;
using HousingProject.Core.Models.RentMonthly;
using HousingProject.Core.Models.RentPayment;
using HousingProject.Core.ViewModel;
using HousingProject.Core.ViewModel.Rentee;
using HousingProject.Core.ViewModel.Rentpayment;
using HousingProject.Infrastructure.CRUDServices.MainPaymentServices;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
using HousingProject.Infrastructure.Interfaces.IUserExtraServices;
using HousingProject.Infrastructure.Response.payment_ref;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousingProject.Architecture.Services.Rentee.Services
{
    public class TenantServices : ITenantServices
    {
        private readonly HousingProjectContext _context;
        private readonly IEmailServices _iemailservvices;
        private readonly ILogger<ITenantServices> _logger;
        private readonly ILoggedIn _loggedIn;
        private readonly IServiceScopeFactory _scopeFactory;
        public readonly IRegistrationServices _registrationServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserExtraServices _userExtraServices;
        private readonly IpaymentServices _paymentservice;

        public TenantServices(
          HousingProjectContext context,
          IHttpContextAccessor httpContextAccessor,
          IEmailServices iemailservvices,
          IRegistrationServices registrationServices,
          ILoggedIn loggedIn,
          IServiceScopeFactory scopeFactory,
          ILogger<ITenantServices> logger,
          IUserExtraServices userExtraServices,
          IpaymentServices paymentservice
        )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _iemailservvices = iemailservvices;
            _registrationServices = registrationServices;
            _loggedIn = loggedIn;
            _scopeFactory = scopeFactory;
            _logger = logger;
            _userExtraServices = userExtraServices;
            _paymentservice = paymentservice;
        }

        // get loggin user
        public async Task<RegistrationModel> LoggedInUser()
        {
            var currentuserid =
              _httpContextAccessor
              .HttpContext
              .User
              .Claims
              .Where(x => x.Type == "Id")
              .Select(p => p.Value)
              .FirstOrDefault();
            var loggedinuser =
              await _context
              .RegistrationModel
              .Where(x => x.Id == currentuserid)
              .FirstOrDefaultAsync();
            return loggedinuser;
        }



        public async Task<BaseResponse> Register_Rentee(Rentee_RegistrationViewModel RenteeVm)
        {

            var loggeinuserr = LoggedInUser().Result;
            if (_loggedIn.LoggedInUser().Result.Is_Tenant || !_loggedIn.LoggedInUser().Result.Is_Admin)
            {
                return new BaseResponse { Code = "124", ErrorMessage = "You do not have permision to do this" };
            }

            try
            {
                using( var scope= _scopeFactory.CreateScope())
                { 
                if (!(loggeinuserr.Is_Agent || !loggeinuserr.Is_Landlord))

                {
                    return new BaseResponse { Code = "129", ErrorMessage = "You do not have permission to do this" };
                }
                if (RenteeVm.FirstName == "" && RenteeVm.LastName == "")

                {
                    return new BaseResponse
                    {
                        Code = "408",
                        ErrorMessage = " Names cann't be empty"
                    };
                }
                if (RenteeVm.Email == "")
                {
                    return new BaseResponse
                    {
                        Code = "409",
                        ErrorMessage = " Email cannot be empty"
                    };
                }
                var usermodel = new RegisterViewModel
                {
                    FirstName = RenteeVm.FirstName,
                    LasstName = RenteeVm.LastName,
                    Email = RenteeVm.Email,
                    IdNumber = RenteeVm.FirstName,
                    PhoneNumber = RenteeVm.Agent_PhoneNumber,
                    Password = "Password@1234",
                    RetypePassword = "Password@1234",
                    BirthDate = "00/00/00",
                    Is_Tenant = true,

                };
                var houseexist = await _context.House_Registration
                    .Where(y => y.HouseiD == RenteeVm.HouseiD)
                    .FirstOrDefaultAsync();

                if (houseexist == null)
                {
                    return new BaseResponse { Code = "150", ErrorMessage = "House does not exist" };
                }
                var resp = await _registrationServices.UserRegistration(usermodel);
                if (resp.Code == "200")
                {
                    await Update_unitStatus(RenteeVm.Appartment_DoorNumber,
                        houseexist.House_Name, houseexist.HouseiD, RenteeVm.Email);
                    var emailbody = new UserEmailOptions
                    {
                        UserName = RenteeVm.FirstName,
                        PayLoad = "sent mail test",
                        ToEmail = RenteeVm.Email
                    };
                    await _iemailservvices.SentdirectlytonewTenant(emailbody);
                }
                else
                {
                    return new BaseResponse { Code = "170", ErrorMessage = "Tenant not created as a user" };
                }
                var renteemodel = new TenantClass
                {
                    FirstName = RenteeVm.FirstName,
                    LastName = RenteeVm.LastName,
                    HouseFloor = RenteeVm.HouseFloor,
                    Email = RenteeVm.Email,
                    CreatedBy = loggeinuserr.Email,
                    Cars = RenteeVm.Cars,
                    ServicesFees = RenteeVm.ServicesFees,
                    Rentee_PhoneNumber = RenteeVm.Rentee_PhoneNumber,
                    BedRoom_Number = RenteeVm.BedRoom_Number,
                    House_Rent = RenteeVm.House_Rent,
                    Agent_PhoneNumber = RenteeVm.Agent_PhoneNumber,
                    BuildingCareTaker_PhoneNumber = RenteeVm.BuildingCareTaker_PhoneNumber,
                    HouseiD = RenteeVm.HouseiD,
                    Appartment_DoorNumber = RenteeVm.Appartment_DoorNumber,
                    Number0f_Occupants = RenteeVm.Number0f_Occupants,
                    Active = true
                };
                _context.Add(renteemodel);
                await _context.SaveChangesAsync();
                var loggedinuser = LoggedInUser().Result;
                var emails = loggedinuser.FirstName;
                var creatorusername = LoggedInUser().Result;
                var sendbody = new UserEmailOptions
                {
                    UserName = loggedinuser.FirstName,
                    PayLoad = "sent mail test",
                    ToEmail = loggedinuser.Email
                };

                await _iemailservvices.newtenantemail(sendbody);
                return new BaseResponse
                {
                    Code = "200",
                    SuccessMessage = "Succesfully registered tenant",

                };


                    }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "300", ErrorMessage = ex.Message };
            }
        }

        public async Task<IEnumerable<TenantClass>> GetAllRenteess()
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                return await scopedcontext.TenantClass.Where(t=>t.Active).OrderByDescending(x => x.DateCreated)
                    .OrderByDescending(x => x.DateCreated).ToListAsync();
            }
           
        }

        //get element by id
        public async Task<BaseResponse> GetTenantById(int tenantId)
        {
            if (_loggedIn.LoggedInUser().Result.Is_Tenant)
            {
                return new BaseResponse { Code = "124", ErrorMessage = "You do not have permision to do this" };
            }
            var tenant = await _context.TenantClass.Where(x => x.RenteeId == tenantId).FirstOrDefaultAsync();
            try
            {
                if (tenant != null)
                {
                    return new BaseResponse { Code = "200", Body = tenant };
                }
                else
                {
                    return new BaseResponse { Code = "104", ErrorMessage = "The tenant doesn not exist" };
                }
            }
            catch (Exception ex)
            {
                foreach (var error in ex.Message)
                {
                    return new BaseResponse { Code = "016", ErrorMessage = error.ToString() };
                }
                return new BaseResponse { Code = "017", ErrorMessage = "Something else happened " };
            }
        }

        public async Task<BaseResponse> GetTenantSummary(int houseId, int tenantId)
        {
            try
            {
                var user = LoggedInUser().Result;
                var gethouseid =
                  await _context
                  .House_Registration
                  .Where(x => x.HouseiD == houseId)
                  .FirstOrDefaultAsync();

                if (gethouseid == null)
                {
                    return new BaseResponse
                    {
                        Code = "000",
                        ErrorMessage = "The house does not exist"
                    };
                }
                var tenant =
                  await _context
                  .TenantClass
                  .Where(x => x.RenteeId == tenantId)
                  .FirstOrDefaultAsync();
                var arears = tenant.House_Rent - tenant.RentPaid;

                if (arears >= 1)
                {
                    return new BaseResponse { Code = "200", SuccessMessage = $"You have  arrears of {arears} Ksh" };
                }
                if (gethouseid.HouseiD == tenant.HouseiD)
                {
                    var summaryobjects = new TenantSummary
                    {
                        HouseiD = gethouseid.HouseiD,
                        AgentName = gethouseid.CreatorNames,
                        HouseDoornumber = tenant.Appartment_DoorNumber,
                        HouseRent = tenant.House_Rent,
                        RentArrears = tenant.House_Rent - tenant.RentPaid,
                        overpayment = tenant.RentOverpayment,
                        TenantId = tenant.RenteeId,
                        FirstName = tenant.FirstName,
                        LastName = tenant.LastName,
                    };

                    return new BaseResponse
                    {
                        Code = "200",
                        Body = summaryobjects
                    };
                }
                else if (gethouseid.HouseiD != tenant.HouseiD)
                {
                    return new BaseResponse
                    {
                        Code = "000",
                        ErrorMessage = "No  rent summary exists for such a tenant "
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    ErrorMessage = ex.Message
                };
            }
            return new BaseResponse { };
        }


        public async Task<BaseResponse> UpdateRentpaid(int tenantid, float rentadded)
        {
            if (_loggedIn.LoggedInUser().Result.Is_Tenant)
            {

                return new BaseResponse { Code = "124", ErrorMessage = "You do not have permision to do this" };
            }

            var tenant =
              await _context
              .TenantClass
              .Where(x => x.RenteeId == tenantid)
              .FirstOrDefaultAsync();
            if (tenant == null)
            {

                return new BaseResponse
                {
                    Code = "0000",
                    ErrorMessage = "Tenant does not exists"
                };
            }
            try
            {

                // var fullnames = tenant.FirstName + " " + tenant.LastName;
                tenant.RentPaid = +rentadded;

                _context.TenantClass.Update(tenant);

                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    Code = "200",
                    SuccessMessage = $"Successfully updated rent details for {rentadded}"
                };

            }
            catch (Exception ex)
            {

                return new BaseResponse
                {
                    Code = "000",
                    ErrorMessage = ex.Message
                };
            }


        }


        public async Task<IEnumerable> GetTenantByHouseid(int houseid)
        {


            var houselist = await _context.TenantClass.Where(x => x.HouseiD == houseid  && x.Active)
                .OrderByDescending(x => x.DateCreated)
               .ToListAsync();

            return houselist;

        }

        public async Task<BaseResponse> TenanttotalRent(int tenantId)
        {

            try
            {
                var tenantpaymenttable = await _context.Rentpayment.Where(x => x.TenantId == tenantId).OrderByDescending(z => z.Datepaid).ToListAsync();
                var tenantcount = tenantpaymenttable.Count;
                var tenantintenantclass = await _context.TenantClass.Where(x => x.RenteeId == tenantId).FirstOrDefaultAsync();
                var rentamount = tenantintenantclass.House_Rent;
                var renttopaid = rentamount * tenantcount;
                return new BaseResponse { Code = "200", Totals = Convert.ToInt32(renttopaid) };
            }
            catch (Exception ex)
            {
                foreach (var error in ex.Message)
                {
                    return new BaseResponse { Code = "110", ErrorMessage = error.ToString() };
                }
            }
            return new BaseResponse { Code = "120", ErrorMessage = "Something new happened" };
        }
        public async Task<BaseResponse> RentTotal(int tenantid)
        {



            try
            {

                using (var scope = _scopeFactory.CreateAsyncScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var alltenantamount = await scopedcontext.PayRent.Where(x => x.TenantId == tenantid).OrderByDescending(y => y.CreatedOn).ToListAsync();
                    var tenanttotalRent = await scopedcontext.PayRent.Where(x => x.TenantId == tenantid).SumAsync(y => y.RentAmount);
                    return new BaseResponse { Code = "200", Body = tenanttotalRent };
                }
            }
            catch (Exception ex)
            {
                foreach (var error in ex.Message)
                {
                    return new BaseResponse { Code = "108", ErrorMessage = error.ToString() };
                }
                return new BaseResponse { Code = "110", ErrorMessage = "something foreign happened" };
            }
        }


        public async Task<IEnumerable> rentpaymentList(int tenantId)
        {
            var tenantlist = await _context.Rentpayment.Where(x => x.TenantId == tenantId).OrderByDescending(a => a.Email).ToListAsync();
            return tenantlist;
        }

        public async Task<BaseResponse> updateTenantRent(int tenantId, RentpaymentViewmodel vm)
        {
            try
            {
                var tenant = await _context.TenantClass.Where(x => x.RenteeId == tenantId).FirstOrDefaultAsync();
                var sentbody = new Rentpayment
                {
                    Email = tenant.Email,
                    RentPaid = vm.RentPaid,
                    TenantId = tenant.RenteeId,
                    Month = vm.Month,
                    HouseId = tenant.HouseiD,
                };
                await _context.AddAsync(sentbody);
                await _context.SaveChangesAsync();
                return new BaseResponse { Code = "200", SuccessMessage = $"Rent updated successfully for {tenant.FirstName} {tenant.LastName}" };
            }
            catch (Exception e)
            {
                foreach (var error in e.Message)
                {
                    return new BaseResponse { Code = "106", ErrorMessage = error.ToString() };
                }
                return new BaseResponse { Code = "108", ErrorMessage = "something foreign happened" };
            }

        }

        public async Task<BaseResponse> GetLoggedInTenant()
        {
            var user = LoggedInUser().Result;
            try
            {
                if (user == null)
                {
                    return new BaseResponse { Code = "190", ErrorMessage = "something happened" };
                }
                var loggedinTenant = await _context.TenantClass.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
                if (loggedinTenant == null)
                {
                    return new BaseResponse { Code = "140", ErrorMessage = "Tenant was not found " };
                }
                if (loggedinTenant.Email == user.Email)
                {
                    if (!user.Is_Tenant)
                    {
                        return new BaseResponse { Code = "235", ErrorMessage = "Kindly use the email sent to activate your Tenant account" };
                    }
                }
                return new BaseResponse { Code = "200", Body = loggedinTenant };
            }
            catch (Exception ex)
            {
                foreach (var error in ex.Message)
                {
                    return new BaseResponse { Code = "130", ErrorMessage = error.ToString() };
                }
            }
            return new BaseResponse { Code = "167", ErrorMessage = "A foreign error was encountered" };
        }

        public async Task<BaseResponse> GetLogeedInTenantHouse()
        {
            var user = LoggedInUser().Result;
            var loggedinTenant = await _context.TenantClass.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
            try
            {
                if (loggedinTenant == null)
                {
                    return new BaseResponse { Code = "345", ErrorMessage = "This tenant does not exist" };
                }
                var houseoftenant = await _context.House_Registration.Where(x => x.HouseiD == loggedinTenant.HouseiD).FirstOrDefaultAsync();

                if (houseoftenant == null)
                {
                    return new BaseResponse { Code = "140", ErrorMessage = "The tent is not registered unser any house" };
                }

                return new BaseResponse { Code = "200", Body = houseoftenant };
            }
            catch (Exception ex)
            {
                foreach (var error in ex.Message)
                {
                    return new BaseResponse { Code = "234", ErrorMessage = error.ToString() };
                }
            }
            return new BaseResponse { Code = "000", ErrorMessage = "Something foreighn happened" };
        }

        public async Task<BaseResponse> SpecificTenantReminderonRentPayment(int tenantid)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    var tenantExists = await scopedcontext.TenantClass.Where(x => x.RenteeId == tenantid).FirstOrDefaultAsync();
                    if (tenantExists == null)
                    {
                        return new BaseResponse { Code = "180", ErrorMessage = "Tenant does not exist" };
                    }

                    var emailbody = new TenantReminderEmail
                    {
                        ToEmail = tenantExists.Email,
                        UserName = tenantExists.FirstName + tenantExists.LastName,
                        Message = $"Dear {tenantExists.FirstName},Kindly be reminded that  your rent pay day is {tenantExists.RentPayDay}"

                    };

                    var response = await _iemailservvices.SendTenantEmailReminderOnRentPayment(emailbody);
                    if (response.Code == "200")
                    {
                        await ReminderSentEntry(tenantExists.RenteeId);
                        return new BaseResponse { Code = "200", SuccessMessage = "Reminder sent to tenant successfully " };
                    }

                    else
                    {
                        return new BaseResponse { Code = "480", ErrorMessage = "Reminder not sent please try again" };

                    }

                }

            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "150", ErrorMessage = ex.Message };
            }


        }

        public async Task<BaseResponse> NotificationonRentPaymentDay()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    var alltentnats = await scopedcontext.TenantClass.OrderByDescending(v => v.DateCreated).ToListAsync();

                    foreach (var tenant in alltentnats)
                    {
                        var emailbody = new EmailNotificationOnRentPayment
                        {
                            ToEmail = tenant.Email,
                            UserName = tenant.FirstName + tenant.LastName,
                            PaymentDate = tenant.RentPayDay,
                            sentDate = DateTime.Now,
                            RenTAmount = tenant.House_Rent,
                            Message = $"Hi Kindly be reminded that your rent  amount of {tenant.House_Rent} is payable on or before {tenant.RentPayDay}, reach out on {tenant.BuildingCareTaker_PhoneNumber} for any further enquiry, or request of extension"
                        };
                    }
                    return new BaseResponse();
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "140", ErrorMessage = ex.Message };

            }

        }
        public async Task<BaseResponse> UpdateRentPayday(DateTime rentpaydate, string email)
        {

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var tenantexists = await scopedcontext.TenantClass
                        .Where(t => t.Email == email).FirstOrDefaultAsync();

                    if (tenantexists == null)
                    {
                        return new BaseResponse { Code = "190", ErrorMessage = "Tenant does not exist" };
                    }

                    tenantexists.RentPayDay = rentpaydate;
                    scopedcontext.UpdateRange(tenantexists);
                    await scopedcontext.SaveChangesAsync();
                    return new BaseResponse { Code = "200", ErrorMessage = "Successfully updated tenant rent" };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "170", ErrorMessage = ex.Message };
            }
        }


        public async Task AutomtedRentNotiication()
        {
            try
            {
                _logger.LogInformation("_______________________starting______________ rent __________________emailing ________________________");
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var alltenants = await scopedcontext.TenantClass.OrderByDescending(y => y.DateCreated).ToListAsync();
                    if (alltenants == null)
                    {
                        _logger.LogInformation("Tenants do not exist");
                    }
                    foreach (var singletenant in alltenants)
                    {
                        if (singletenant.ReminderSent == false)
                        {
                            var sendmail = new AutomaticMessaging
                            {
                                TenantNmes = singletenant.FirstName + " " + singletenant.LastName,
                                ToEmail = singletenant.Email,
                                SentDate = singletenant.RentPayDay,
                                Meessage = $"Hi {singletenant.FirstName}  {singletenant.LastName} ,just a kind reminder," +
                                $"Kindly be reminded that your rent paydate is {string.Format("{0:dd/MM/yyyy}", singletenant.RentPayDay)}"
                            };
                            //var sendmail = new AutomaticMessaging
                            //{
                            //    TenantNmes = "Brian Otieno",
                            //    ToEmail = "osano3204@gmail.com",
                            //    // RentDate = singletenant.RentPayDay,
                            //    SentDate = DateTime.Now,
                            //    Meessage = $"Hi Judas Iscariot ,just a kind reminder," +
                            //        $"Kindly be reminded that your rent paydate is {string.Format("{0:dd/MM/yyyy}", DateTime.Now)}"
                            //};
                            var resp = await _iemailservvices.notificationOnRentPaymeentDay(sendmail);

                            if (resp.Code == "200")
                            {
                                singletenant.ReminderSent = true;
                                var sendcount = await scopedcontext.TenantClass.Where(t => t.RenteeId == singletenant.RenteeId).FirstOrDefaultAsync();
                                sendcount.RemindersentCount = +1;
                                singletenant.RemindersentCount = sendcount.RemindersentCount;
                                scopedcontext.Update(singletenant);
                                await scopedcontext.SaveChangesAsync();
                                _logger.LogInformation($" Email sent to ___ {sendmail.TenantNmes}, " +
                                $"{sendmail.ToEmail} ___ at ____ {string.Format("{0:dd/MM/yyyy}", DateTime.Now)}________");
                            }
                            else
                            {
                                _logger.LogInformation("Email not sent to tenant");
                            }
                        }
                        else
                        {
                            _logger.LogInformation("Email already sent ");
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                _logger.LogInformation($"Failed with error _______________{ex.Message} ____");

            }


        }

        //update reminder sent table 


        public async Task<BaseResponse> ReminderSentEntry(int tenantid)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    //currently logged in user
                    var currenuser = _loggedIn.LoggedInUser().Result;

                    //gettenant receiving mail sent
                    var tenantexists = await scopedcontext.TenantClass.Where(t => t.RenteeId == tenantid).FirstOrDefaultAsync();
                    if (tenantexists == null)
                    {
                        return new BaseResponse { Code = "160", ErrorMessage = "Tenant does not exist" };
                    }
                    //find related house
                    var houseexists = await scopedcontext.House_Registration.Where(h => h.HouseiD == tenantexists.HouseiD).FirstOrDefaultAsync();
                    if (houseexists == null)
                    {
                        return new BaseResponse { Code = "140", ErrorMessage = "House does not exist" };
                    }
                    var remindertable = new ReminderSentDate
                    {
                        TenantId = tenantexists.RenteeId,
                        TenantNames = tenantexists.FirstName + " " + tenantexists.LastName,
                        TenantEmail = tenantexists.Email,
                        HouseName = houseexists.House_Name,
                        ReminderSent = "Yes",
                        DoorNumber = tenantexists.Appartment_DoorNumber,
                        SendByNames = currenuser.FirstName + "  " + currenuser.LasstName,
                        SentByEmail = currenuser.Email,
                        HouseId = houseexists.HouseiD
                    };

                    //save to database
                    await scopedcontext.AddAsync(remindertable);
                    await scopedcontext.SaveChangesAsync();
                    return new BaseResponse { Code = "200", SuccessMessage = "Reminder sent table updated succesfully" };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "140", ErrorMessage = ex.Message };
            }
        }

        //get all reminder sent  
        public async Task<BaseResponse> AllRemindersSent(int houseid)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var currentuser = _loggedIn.LoggedInUser().Result;
                    //all reminders sent on rent payment
                    var allremindersent = await scopedcontext.ReminderTable.Where(h => h.HouseId == houseid).OrderByDescending(h => h.DateSent).ToListAsync();
                    if (allremindersent == null)
                    {
                        return new BaseResponse { Code = "160", SuccessMessage = "There are no reminders sent to show" };
                    }
                    return new BaseResponse { Code = "200", SuccessMessage = "Successfully queried", Body = allremindersent };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "140", ErrorMessage = ex.Message };
            }
        }
        public async Task Update_unitStatus(int doornumber, string housename, int houseid,string email)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontet = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    //unit exists
                    var house_unit_exists = await scopedcontet.HouseUnitsStatus
                        .Where(s => s.DoorNumber == doornumber && s.HouseName == housename).FirstOrDefaultAsync();
                    if (house_unit_exists == null)
                    {
                        _logger.LogInformation("_house unit does not exist ");
                    }
                    house_unit_exists.Occupied = true;
                    house_unit_exists.HouseID = houseid;
                    house_unit_exists.Tenant_Email = email;

                    scopedcontet.Update(house_unit_exists);
                    await scopedcontet.SaveChangesAsync();
                    _logger.LogInformation("_successully updated house status ");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"___________error message {ex.Message}");
                //return new BaseResponse { Code = "140", ErrorMessage = ex.Message };
            }
        }
        public async Task<BaseResponse> PayingRent(int tenantid, decimal rentamount)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    if (tenantid <= 0)
                    {
                        return new BaseResponse { Code = "144", ErrorMessage = "Tenant id cannot be null" };
                    }
                    if (rentamount <= 0)
                    {
                        return new BaseResponse { Code = "190", ErrorMessage = "Rent amount cannot be null" };
                    }
                    var tenantexists = await scopedcontext.TenantClass
                        .Where(t => t.RenteeId == tenantid).FirstOrDefaultAsync();
                    if (tenantexists == null)
                    {
                        return new BaseResponse { Code = "220", ErrorMessage = "Tenant does not exist" };
                    }
                    var newrent = new PayRent
                    {
                        // Payrentid= tenantexists.RenteeId,
                        TenantId = tenantexists.RenteeId,
                        RentAmount = rentamount,
                        Completed = false,
                        PhoneNumber = tenantexists.Rentee_PhoneNumber,
                        HouseID = tenantexists.HouseiD
                    };
                    var rent_payment = await _paymentservice.STk_Push(tenantexists.Rentee_PhoneNumber, rentamount);
                    var trans_ref = rent_payment.internalref;
                    if (rent_payment.Code == "200")
                    {
                        var generatedref = await GetGeneratedref();
                        newrent.InternalReference = trans_ref;
                        newrent.Merchant_Request_ID = rent_payment.message;
                        newrent.Status = "PROCCESSING";
                        newrent.ProviderReference = "N/A";
                        await scopedcontext.AddAsync(newrent);
                        await scopedcontext.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "Rent payment initiated successfully " };
                    }
                    else
                    {
                        var generatedref = await GetGeneratedref();
                        newrent.InternalReference = trans_ref;
                        newrent.Status = "FAILED";
                        newrent.ProviderReference = "N/A";
                        await scopedcontext.AddAsync(newrent);
                        await scopedcontext.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "Rent payment FAILED " };
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "345", ErrorMessage = ex.Message };
            }
        }

        public async Task<BaseResponse> GetAllTenantPayments(int tenantid)
        {

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    //tenant exists
                    var tenantexists = await scopedcontext.TenantClass.Where(t => t.RenteeId == tenantid).FirstOrDefaultAsync();
                    if (tenantexists == null)
                    {
                        return new BaseResponse { Code = "140", ErrorMessage = "Tenant does not exist" };
                    }

                    //all payments
                    var alltenantpayments = await scopedcontext.PayRent.Where(t => t.TenantId == tenantexists.RenteeId).OrderByDescending(x => x.CreatedOn).ToListAsync();
                    if (alltenantpayments == null)
                    {
                        return new BaseResponse { Code = "190", ErrorMessage = "tenant has no payment record" };
                    }
                    return new BaseResponse { Code = "200", SuccessMessage = "Succesfully queried ", Body = alltenantpayments };
                }

            }
            catch (Exception ex)
            {
                //return new BaseResponse { Code = "130", ErrorMessage = ex.Message , Body= ex.StackTrace};
                return new BaseResponse { Code = "109", ErrorMessage = ex.Message, Body = ex.StackTrace };
            }
        }
        public async Task<string> GetGeneratedref()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    int length = 11;
                    var paymentref = "LH_" + _userExtraServices.GenerateReferenceNumber(length);
                    //check reference exists
                    var referenceexists = await scopedcontext.PayRent.Where(y => y.InternalReference == paymentref).ToListAsync();
                    if (referenceexists.Count >= 1)
                    {
                        await GetGeneratedref();
                    }
                    return paymentref;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public async Task<BaseResponse> GetHouseUnitBodyById(int houseuintid)
        {
            try
            {
                var houseUnitexists = await _context.HouseUnitsStatus.Where(h => h.HouseidstatusID == houseuintid).FirstOrDefaultAsync();
                if (houseUnitexists == null)
                {
                    return new BaseResponse { Code = "130", ErrorMessage = "House unit does not exist", Body = houseUnitexists };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { ErrorMessage = ex.Message };
            }
            return new BaseResponse { Code = "200", SuccessMessage = "Queried successfully" };
        }
        public async Task<BaseResponse> RequestRentDelay(string requestdate, string addtionalDetails)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    if (requestdate == null)
                    {
                        return new BaseResponse { Code = "190", ErrorMessage = "Date field cannot be empty" };
                    }
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var user = await _loggedIn.LoggedInUser();
                    var tenantexists = await scopedcontext.TenantClass.Where(t => t.Email == user.Email).FirstOrDefaultAsync();
                    if (tenantexists == null)
                    {
                        new BaseResponse { Code = "140", ErrorMessage = "Tenant does not exist" };
                    }
                    else
                    {
                        var newdelayrequest = new RentDelayRequestTable
                        {
                            AdditionDetails = addtionalDetails,
                            DateRequested = Convert.ToDateTime(requestdate),
                            TenantRentPaymentDate = tenantexists.RentPayDay,
                            RequesterId = user.Id,
                            Requestermail = tenantexists.Email,
                            RequesterNames = tenantexists.FirstName + " " + tenantexists.LastName,
                            HouseId = tenantexists.HouseiD,
                            DoorNumber = tenantexists.Appartment_DoorNumber,
                            Status = "PENDING"
                        };

                        var currentDate = DateTime.Now;

                        if (newdelayrequest.DateRequested < currentDate)
                        {
                            return new BaseResponse { Code = "178", ErrorMessage = "Kindly make sure your requested payment  date is not an already passed date" };
                        }
                        await scopedcontext.AddAsync(newdelayrequest);
                        await scopedcontext.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = $"successfully submited a request on {newdelayrequest.RequestDate} for rent delay payment on {Convert.ToString(newdelayrequest.DateRequested)} " };
                    }
                    return new BaseResponse();
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "120", ErrorMessage = ex.Message };
            }
        }

        public async Task<BaseResponse> GetAll_DelayRequests_By_HouseId(int houseid)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var all_delay_requests = await scopedcontext.RentDelayRequestTable.Where(u => u.HouseId == houseid).OrderByDescending(y => y.DateRequested).ToListAsync();
                    if (all_delay_requests == null)
                    {
                        new BaseResponse { Code = "140", ErrorMessage = "No delay requests available" };
                    }
                    return new BaseResponse { Code = "200", SuccessMessage = "Successfully queried", Body = all_delay_requests };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "109", ErrorMessage = ex.Message };
            }
        }


        public async Task<BaseResponse> GetAll_DelayRequests_By_TenantEmail(string tenantemail)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var all_User_delay_requests = await scopedcontext.RentDelayRequestTable.
                        Where(u => u.Requestermail == tenantemail).OrderByDescending(y => y.RequestDate)
                        .ToListAsync();
                    if (all_User_delay_requests == null)
                    {
                        new BaseResponse { Code = "140", ErrorMessage = "No delay requests available" };
                    }
                    return new BaseResponse { Code = "200", SuccessMessage = "Successfully queried", Body = all_User_delay_requests };
                }
            }
            catch (Exception ex)
            {

                return new BaseResponse { Code = "109", ErrorMessage = ex.Message };
            }

        }

        public async Task<BaseResponse> GetDelayRequestsBystatus(string status, int houseid)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    if (status == "APPROVED")
                    {
                        var approved_request = await scopedcontext.RentDelayRequestTable.Where(u => u.Status == "APPROVED" || u.HouseId == houseid).OrderByDescending(y => y.DateRequested).ToListAsync();

                        if (approved_request == null)
                        {

                            return new BaseResponse { Code = "160", ErrorMessage = "Nothing to show " };
                        }
                        return new BaseResponse { Code = "200", SuccessMessage = "successfully queried", Body = approved_request };
                    }
                    else if (status == "PENDING")
                    {
                        var pending_request = await scopedcontext.RentDelayRequestTable.Where(u => u.Status == "PENDING" || u.HouseId == houseid).OrderByDescending(u => u.DateRequested).ToListAsync();
                        if (pending_request == null)
                        {

                            return new BaseResponse { Code = "160", ErrorMessage = "Nothing to show " };
                        }
                        return new BaseResponse { Code = "200", SuccessMessage = "successfully queried", Body = pending_request };
                    }
                    else if (status == "DECLINED")
                    {
                        var declined_requests = await scopedcontext.RentDelayRequestTable
                            .Where(u => u.Status == "DECLINED" || u.HouseId == houseid)
                            .OrderByDescending(c => c.DateRequested).ToListAsync();
                        if (declined_requests == null)
                        {

                            return new BaseResponse { Code = "160", ErrorMessage = "Nothing to show " };
                        }
                        return new BaseResponse { Code = "200", SuccessMessage = "successfully queried", Body = declined_requests };
                    }

                    return new BaseResponse();
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "190", ErrorMessage = ex.Message };
            }
        }

        public async Task<BaseResponse> ApproveRequest(int requestid)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var pending_requestfound = await scopedcontext.RentDelayRequestTable.Where(u => u.Status == "PENDING" || u.delay_request_id == requestid).FirstOrDefaultAsync();
                    if (pending_requestfound == null)
                    {
                        return new BaseResponse { Code = "160", ErrorMessage = "Nothing to show " };
                    }
                    pending_requestfound.Status = "APPROVED";
                    scopedcontext.Update(pending_requestfound);
                    await scopedcontext.SaveChangesAsync();
                    return new BaseResponse { Code = "200", SuccessMessage = "successfully approved" };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "190", ErrorMessage = ex.Message };
            }
        }

        //reject request
        public async Task<BaseResponse> RejectRequest(int requestid)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    var pending_requestfound = await scopedcontext.RentDelayRequestTable.Where(u => u.Status == "PENDING" || u.delay_request_id == requestid).FirstOrDefaultAsync();
                    if (pending_requestfound == null)
                    {
                        return new BaseResponse { Code = "160", ErrorMessage = "Nothing to show " };
                    }

                    pending_requestfound.Status = "REJECTED";

                    scopedcontext.Update(pending_requestfound);
                    await scopedcontext.SaveChangesAsync();
                    return new BaseResponse { Code = "200", SuccessMessage = "successfully rejected" };

                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "190", ErrorMessage = ex.Message };
            }

        }
        public async Task<BaseResponse> GetDelayRequetsByHouseIDandStatus(int houseid, string requestStatus)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var enddate = DateTime.Now;
                    var startdate = DateTime.Now.AddDays(-80);
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var requests_exists = await scopedcontext.RentDelayRequestTable.
                        Where(y => y.Status == requestStatus && y.HouseId == houseid
                       ).OrderByDescending(x => x.DateRequested).ToListAsync();
                    if (requests_exists == null)
                    {
                        return new BaseResponse { Code = "160", ErrorMessage = "Nothing to show " };
                    }

                    return new BaseResponse { Code = "200", Body = requests_exists };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "190", ErrorMessage = ex.Message };
            }
        }


        public async Task MonthlyRentfn()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var alltenants = await scopedcontext.TenantClass.ToListAsync();
                    foreach (var tenantdata in alltenants)
                    {
                        var houseexists = await scopedcontext.House_Registration.Where(y => y.HouseiD == tenantdata.HouseiD).FirstOrDefaultAsync();


                        var newmnthpay = new Rent_Monthly_Update
                        {
                            Tenantid = tenantdata.RenteeId,
                            Tenantnames = tenantdata.FirstName + " " + tenantdata.LastName,
                            Tenant_Email = tenantdata.Email,
                            Month = DateTime.Now.ToString("MMMM"),
                            Year = DateTime.Now.Year,
                            RentAmount = tenantdata.House_Rent,
                            HouseName = houseexists.House_Name,
                            Updated_This_Month = true,
                            DateUpdated = DateTime.Now,
                            House_ID = tenantdata.HouseiD,
                            PhoneNumber = tenantdata.Rentee_PhoneNumber
                        };
                        var rent_value_exists = await scopedcontext.Rent_Monthly_Update
                        .Where(y => y.Tenantid == newmnthpay.Tenantid).OrderByDescending(y => y.DateUpdated).FirstOrDefaultAsync();


                        if (rent_value_exists == null || rent_value_exists.Balance == 0)
                        {
                            newmnthpay.Balance = tenantdata.House_Rent;
                        }
                        else
                        {
                            newmnthpay.Balance = rent_value_exists.Balance + tenantdata.House_Rent;
                        }

                        await scopedcontext.AddAsync(newmnthpay);
                        await scopedcontext.SaveChangesAsync();
                        _logger.LogInformation($"saved successfully @ {Convert.ToString(DateTime.Now)} ");

                    }

                }

            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex.StackTrace);
                _logger.LogInformation(ex.Message);
            }
        }
        public async Task Reset_Updated_this_month()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var all_tenants = await scopedcontext.Rent_Monthly_Update
                        .Where(y => y.DateCreated > DateTime.Now.AddDays(-30) && y.Updated_This_Month == true).ToListAsync();

                    if (all_tenants == null)
                    {
                        _logger.LogInformation("No tenants found");
                    }

                    foreach (var existing_tenant in all_tenants)
                    {

                        existing_tenant.Updated_This_Month = false;
                        scopedcontext.Update(existing_tenant);
                        await scopedcontext.SaveChangesAsync();

                        _logger.LogInformation("Successfully updated back to false on monthly rent payment");

                    }


                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }


        }

        public async Task<Payments_Reference_Response> Get_Monthly_Rent_Update(int house_id)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var start_time = DateTime.Now.ToString("MMMM");
                    var current_year = DateTime.Now.Year;
                    var all_monthly_rent_payments = await scopedcontext.Rent_Monthly_Update
                        .Where(y => y.House_ID == house_id && y.Month == start_time && y.Year == current_year)
                        .OrderByDescending(u => u.DateUpdated).ToListAsync();
                    if (all_monthly_rent_payments == null)
                        return new Payments_Reference_Response { Message = "No payment found" };

                    var balance_left = await scopedcontext.Rent_Monthly_Update
                        .Where(y => y.House_ID == house_id && y.Month == start_time && y.Year == current_year)
                        .SumAsync(v => v.Balance);

                    var total_paid = await scopedcontext.Rent_Monthly_Update
                         .Where(y => y.House_ID == house_id && y.Month == start_time && y.Year == current_year)
                         .SumAsync(s => s.Paid);
                    if (all_monthly_rent_payments == null)
                    {
                        return new Payments_Reference_Response { Code = "200", Message = "No payments found", Balance_left = balance_left };
                    }

                    return new Payments_Reference_Response
                    {
                        Code = "200",
                        Message = "Queried successfully",
                        Body = all_monthly_rent_payments,
                        Balance_left = balance_left,
                        Total_paid = total_paid
                    };
                }
            }
            catch (Exception ex)
            {
                return new Payments_Reference_Response { Code = "140", Message = ex.Message };

            }

        }

        public async Task<BaseResponse> Vacant_House_update(int house_id, int door_number)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    //check unit exists
                    var house_unit_exists = await scopedcontext.HouseUnitsStatus
                         .Where(y => y.HouseID == house_id && y.DoorNumber == door_number).FirstOrDefaultAsync();

                    if (house_unit_exists == null)
                    {
                        return new BaseResponse { Code = "190", ErrorMessage = "The house does not exist" };
                    }

                    //tenant exists

                    var tenant_exists = await scopedcontext.TenantClass
                        .Where(y => y.Email == house_unit_exists.Tenant_Email)
                        .FirstOrDefaultAsync();
                    if (tenant_exists == null)
                    {
                        return new BaseResponse { ErrorMessage = "tenant  email doesnt exists or was changed" };
                    }
                    tenant_exists.Active = false;
                    scopedcontext.Update(tenant_exists);
                    await scopedcontext.SaveChangesAsync();
                   
                    house_unit_exists.Occupied = false;
                    scopedcontext.Update(house_unit_exists);
                    await scopedcontext.SaveChangesAsync();
                    return new BaseResponse { Code = "200", SuccessMessage = "Successfully updated  the house" };



                }

            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Code = "140",
                    ErrorMessage = ex.Message
                };
            }
        }
        public async Task<BaseResponse> Get_All_Occupied_House(int house_id)
        {

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var occupied_houses = await scopedcontext
                        .HouseUnitsStatus.Where(y => y.HouseID == house_id && y.Occupied)
                        .OrderByDescending(g=>g.DoorNumber).ToListAsync();

                    if (occupied_houses == null)
                    {
                        return new BaseResponse { Code = "180", ErrorMessage = "Houses not found" };
                    }
                    return new BaseResponse { Code = "200", SuccessMessage = "Queried successfully", Body = occupied_houses };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "190", ErrorMessage = ex.Message };
            }
        }
    }
}
   