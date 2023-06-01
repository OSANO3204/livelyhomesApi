using HousingProject.Architecture.Data;
using HousingProject.Architecture.Interfaces.IEmail;
using HousingProject.Architecture.Interfaces.IRenteeServices;
using HousingProject.Architecture.IPeopleManagementServvices;
using HousingProject.Architecture.Response.Base;
using HousingProject.Architecture.ViewModel.People;
using HousingProject.Core.Models.Email;
using HousingProject.Core.Models.General;
using HousingProject.Core.Models.People;
using HousingProject.Core.Models.People.General;
using HousingProject.Core.Models.ReminderonRentpayment;
using HousingProject.Core.Models.RentPayment;
using HousingProject.Core.ViewModel;
using HousingProject.Core.ViewModel.Rentee;
using HousingProject.Core.ViewModel.Rentpayment;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
using HousingProject.Infrastructure.Interfaces.IUserExtraServices;
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

        public TenantServices(
          HousingProjectContext context,
          IHttpContextAccessor httpContextAccessor,
          IEmailServices iemailservvices,
          IRegistrationServices registrationServices,   
          ILoggedIn loggedIn,
          IServiceScopeFactory scopeFactory,
          ILogger<ITenantServices> logger,
          IUserExtraServices userExtraServices
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
            if (_loggedIn.LoggedInUser().Result.Is_Tenant)
            {
                return new BaseResponse { Code = "124", ErrorMessage = "You do not have permision to do this" };
            }

            try
            {
                if (!(loggeinuserr.Is_Agent || loggeinuserr.Is_Landlord))

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

                var resp = await _registrationServices.UserRegistration(usermodel);
                if (resp.Code == "200")
                {
                    await Update_unitStatus(RenteeVm.Appartment_DoorNumber);
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
            catch (Exception ex)
            {
                return new BaseResponse { Code = "300", ErrorMessage = ex.Message };
            }
        }

        public async Task<IEnumerable<TenantClass>> GetAllRenteess()
        {
            return await _context.TenantClass.OrderByDescending(x => x.DateCreated).ToListAsync();
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

                //if (arears >= 1)
                //{
                //    return new BaseResponse { Code = "200", SuccessMessage = $"You have  arrears of {arears} Ksh" };
                //}
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


            var houselist = await _context.TenantClass.Where(x => x.HouseiD == houseid).OrderByDescending(x => x.DateCreated).ToListAsync();

            return houselist;

        }

        public async Task<BaseResponse> TenanttotalRent(int tenantId)
        {

            try
            {

                var tenantpaymenttable = await _context.Rentpayment.Where(x => x.TenantId == tenantId).ToListAsync();

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
                var tenanttotalRent = await _context.PayRent.Where(x => x.TenantId == tenantid).SumAsync(y => y.RentAmount);
                return new BaseResponse { Code = "200", Body = tenanttotalRent };
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
            var tenantlist = await _context.Rentpayment.Where(x => x.TenantId == tenantId).OrderByDescending(x => x.Datepaid).ToListAsync();
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

                    var alltentnats = await scopedcontext.TenantClass.ToListAsync();

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
                    var alltenants = await scopedcontext.TenantClass.ToListAsync();
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
                        HouseId=houseexists.HouseiD
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
                    var currentuser =  _loggedIn.LoggedInUser().Result;
                    //all reminders sent on rent payment
                    var allremindersent = await scopedcontext.ReminderTable.Where(h=>h.HouseId==houseid).OrderByDescending(d=>d.DateSent).ToListAsync();
                    if (allremindersent == null)
                    {
                        return new BaseResponse { Code = "160", SuccessMessage = "There are no reminders sent to show"};
                    }
                    return new BaseResponse { Code = "200", SuccessMessage = "Successfully queried", Body = allremindersent };
                }

            }
            catch(Exception ex)
            {
                return new BaseResponse { Code = "140", ErrorMessage = ex.Message };
            }
        }
        public async Task Update_unitStatus(int doornumber)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedcontet = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    //unit exists
                    var house_unit_exists = await scopedcontet.HouseUnitsStatus
                        .Where(s => s.DoorNumber == doornumber).FirstOrDefaultAsync();
                    if (house_unit_exists == null)
                    {
                        _logger.LogInformation("_house unit does not exist ");
                    }
                    house_unit_exists.Occupied = true;
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
                    if (rentamount <=0)
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
                               TenantId=tenantexists.RenteeId,
                                RentAmount=rentamount,
                                Status="PROCCESSING",
                                Completed=false,
                                PhoneNumber=tenantexists.Rentee_PhoneNumber,
                                HouseID=tenantexists.HouseiD
                            };
                    int length = 11;
                    var paymentref = "LH_" + _userExtraServices.GenerateReferenceNumber(length);
                    //check reference exists
                    var referenceexists = await scopedcontext.PayRent.Where(y => y.InternalReference == paymentref).FirstOrDefaultAsync();
                    if (referenceexists != null)
                            {
                                int lengtvalues = 3;
                              paymentref="LH_"  + _userExtraServices.GenerateReferenceNumber(length) + _userExtraServices.GenerateReferenceNumber(lengtvalues); 
                            }
                    newrent.InternalReference = paymentref;
                    await scopedcontext.AddAsync(newrent);
                    await scopedcontext.SaveChangesAsync();
                    return new BaseResponse { Code = "200", SuccessMessage = "Rent payment initiated successfully " };
                }           
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "345", ErrorMessage = ex.Message };
            }
        }

        public async Task<BaseResponse>  GetAllTenantPayments(int tenantid)
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
                    var alltenantpayments = await scopedcontext.PayRent.Where(t => t.TenantId == tenantexists.RenteeId).ToListAsync();
                    if (alltenantpayments == null)
                    {
                        return new BaseResponse { Code = "190", ErrorMessage = "tenant has no payment record" };
                    }
                    return new BaseResponse { Code = "200", SuccessMessage = "Succesfully queried ", Body = alltenantpayments };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "130", ErrorMessage = ex.Message };
            }
        }


        public async  Task<BaseResponse> GetHouseUnitBodyById(int houseuintid)
        {
            try
            {
                var houseUnitexists = await _context.HouseUnitsStatus.Where(h => h.HouseidstatusID == houseuintid).FirstOrDefaultAsync();
                if (houseUnitexists == null)
                {
                    return new BaseResponse { Code = "130", ErrorMessage = "House unit does not exist", Body=houseUnitexists };
                }
            }
            catch(Exception ex)
            {
                return new BaseResponse { ErrorMessage= ex.Message};
            }
            return new BaseResponse { Code = "200", SuccessMessage = "Queried successfully" };
        }

    }
}