using HousingProject.Architecture.Data;
using HousingProject.Architecture.IHouseRegistration_Services;
using HousingProject.Architecture.Interfaces.IEmail;
using HousingProject.Architecture.Interfaces.IlogginServices;
using HousingProject.Architecture.IPeopleManagementServvices;
using HousingProject.Architecture.Response.Base;
using HousingProject.Architecture.ViewModel.People;
using HousingProject.Core.Models.Email;
using HousingProject.Core.Models.Houses.Flats.AdminContacts;
using HousingProject.Core.Models.Houses.Flats.House_Registration;
using HousingProject.Core.Models.Houses.HouseAggrement;
using HousingProject.Core.Models.Houses.HouseUnitRegistration;
using HousingProject.Core.Models.Houses.HouseUsers;
using HousingProject.Core.Models.People;
using HousingProject.Core.ViewModel.Aggreement;
using HousingProject.Core.ViewModel.House;
using HousingProject.Core.ViewModel.House.HouseUsersvm;
using HousingProject.Core.ViewModel.HouseUnitRegistrationvm;
using HousingProject.Core.ViewModels;
using HousingProject.Infrastructure.CRUDServices.N_IMages_Services;
using HousingProject.Infrastructure.ExtraFunctions.Checkroles.IcheckRole;
using HousingProject.Infrastructure.ExtraFunctions.RolesDescription;
using HousingProject.Infrastructure.Response;
using HousingProject.Infrastructure.Response.BaseResponses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HousingProject.Architecture.HouseRegistration_Services
{
    public class House_RegistrationServices : IHouse_RegistrationServices
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ICheckroles _checkroles;
        private readonly HousingProjectContext _context;
        private readonly IEmailServices _iemailservices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public static IHostingEnvironment _environment;
        private readonly IRoles _iroles;
        public readonly IRegistrationServices _registrationServices;
        private readonly ILogger<House_RegistrationServices> _logger;
        private readonly IloggedInServices _iloggedInServices;
        private readonly In_ImagesServices _n_imageservics;
        public House_RegistrationServices(
            HousingProjectContext context,
            IEmailServices iemailservices,
            ICheckroles checkroles,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment environment,
            IRoles iroles,
            IRegistrationServices registrationServices,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<House_RegistrationServices> logger,
            IloggedInServices iloggedInServices,
            In_ImagesServices n_imageservics

        )
        {
            _context = context;
            _iemailservices = iemailservices;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
            _iroles = iroles;
            _checkroles = checkroles;
            _registrationServices = registrationServices;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _iloggedInServices = iloggedInServices;
            _n_imageservics = n_imageservics;
        }

        public async Task<RegistrationModel> LoggedInUser()
        {
            var currentuserid = _httpContextAccessor.HttpContext.User.Claims
                .Where(x => x.Type == "Id")
                .Select(p => p.Value)
                .FirstOrDefault();
            var loggedinuser = await _context.RegistrationModel
                .Where(x => x.Id == currentuserid)
                .FirstOrDefaultAsync();
            return loggedinuser;
        }


        public async Task<BaseResponse> TotalHusesManaged(string email)
        {
            var gettotals = await _context.House_Registration.Where(x => x.CreatorEmail == email).ToListAsync();
            var totals = gettotals.Count;
            return new BaseResponse { Code = "200", Totals = totals };
        }

        //end
        public async Task<BaseResponse> Register_House(HouseRegistrationViewModel newvm)
        {

            try
            {
                var currentuser = LoggedInUser().Result;
                if (currentuser.Is_CareTaker && currentuser.Is_Tenant && !currentuser.Is_Landlord && !currentuser.Is_Agent)
                {
                    return new BaseResponse { Code = "159", ErrorMessage = "You cannot register a house, your role is a caretaker and a tenant" };
                }

                if (currentuser.Is_Tenant && !currentuser.Is_Landlord && !currentuser.Is_Agent && !currentuser.Is_CareTaker)
                {
                    return new BaseResponse { Code = "149", ErrorMessage = "You cannot register a house, your role is a tenant" };

                }
                if (currentuser.Is_CareTaker && !currentuser.Is_Landlord && !currentuser.Is_Agent && !currentuser.Is_Tenant)
                {
                    return new BaseResponse { Code = "149", ErrorMessage = "You cannot register a house, your role is a caretaker" };

                }

                //var currentuser = LoggedInUser().Result;
                if (!currentuser.Is_Agent && !currentuser.Is_Landlord)
                {
                    return new BaseResponse { Code = "129", ErrorMessage = "You don't  have access to do this" };
                }
                if (newvm.House_Location == "")
                {
                    return new BaseResponse { Code = "110", ErrorMessage = "House location cannot be null" };
                }
                if (newvm.Total_Units <= 0)
                    {
                        return new BaseResponse { Code = "110", ErrorMessage = "House units cannot be less than or equal to zero" };
                    }
                if (newvm.Owner_Firstname == "")
                    {
                        return new BaseResponse { Code = "110", ErrorMessage = "owner first name cannot be empty" };
                    }
                if (newvm.Owner_LastName == "")
                    {
                        return new BaseResponse { Code = "110", ErrorMessage = "owner last name cannot be empty" };
                    }
                if (newvm.Country == "")
                    {
                        return new BaseResponse { Code = "110", ErrorMessage = "County field cannot be empty" };
                    }
                if (newvm.Estimated_Maximum_Capacity <= 0)
                {
                    return new BaseResponse { Code = "110", ErrorMessage = "Estimated capacity must me higher than zero" };
                }
                if (newvm.Owner_id_Number <= 0)
                {
                    return new BaseResponse { Code = "110", ErrorMessage = "House owner id cannot be null" };
                }

                var house_name = await _context.House_Registration.Where(d => d.House_Name == newvm.House_Name).FirstOrDefaultAsync();
                if (house_name != null)
                {
                    return new BaseResponse
                    {
                        Code = "160",
                        ErrorMessage = "House name already taken " +
                        ", kindly choose another name or  add other distintictive characters "
                    };
                }
                var housereg = new House_Registration
                {
                    Owner_Firstname = newvm.Owner_Firstname,
                    Owner_LastName = newvm.Owner_LastName,
                    Owner_id_Number = newvm.Owner_id_Number,
                    House_Name = newvm.House_Name,
                    Total_Units = newvm.Total_Units,
                    Area = newvm.Area,
                    Country = newvm.Country,
                    House_Location = newvm.House_Location,
                    Estimated_Maximum_Capacity = newvm.Estimated_Maximum_Capacity,
                    EmailSent = false,
                    CreatorEmail = currentuser.Email,
                    CreatorNames = currentuser.FirstName + " " + currentuser.LasstName,
                    UserId = currentuser.Id,
                };

                await _context.House_Registration.AddAsync(housereg);
                await _context.SaveChangesAsync();
                var emails = housereg.CreatorEmail;
                var creatorusername = LoggedInUser().Result;
                var sendbody = new UserEmailOptions
                {
                    UserName = creatorusername.FirstName,
                    PayLoad = "sent mail test",
                    ToEmail = emails
                };

                var result = await _iemailservices.sendEmailOnHouseRegistration(sendbody);
                 if (result.Code == "200")
                    {
                      int fromzero = 0;
                      while (newvm.Total_Units > fromzero)
                        {
                            fromzero++;
                            var saveunit = new HouseUnitsStatus();
                            saveunit.DoorNumber = fromzero;
                            saveunit.HouseName = newvm.House_Name;
                            saveunit.Occupied = false;
                            await _context.AddAsync(saveunit);
                            await _context.SaveChangesAsync();
                        }
                      housereg.EmailSent = true;
                     _context.House_Registration.Update(housereg);
                    await _context.SaveChangesAsync();
                    return new BaseResponse
                    {
                        Code = "200",
                        SuccessMessage = "House  registered successfully "
                    };
                }
                return (new BaseResponse { SuccessMessage = "Failed to send ", });
            }
            catch (Exception ex)
            {
                return new BaseResponse { ErrorMessage = ex.Message };
            }
        }

        public async Task<BaseResponse> Registered_Houses()
        {
            var user = LoggedInUser().Result;
            if (user.Is_Tenant)
            {
                return new BaseResponse { Code = "123", ErrorMessage = "You dont have permission to view this" };
            }
            if (user.IsHouseUsers)
            {
                var usershouselist = Get_HouseUsers_Houses().Result.Body;
                return new BaseResponse { Code = "200", SuccessMessage = "Successful", Body = usershouselist };
            }
            var houselists = await _context.House_Registration
                .Where(x => x.CreatorEmail == user.Email)
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();
            List<HouseListsViewModel> houses = new List<HouseListsViewModel>();
            foreach (var houseitem in houselists)
            {
                var houselistvm = new HouseListsViewModel
                {
                    House_Location = houseitem.House_Location,
                    HouseiD = houseitem.HouseiD,
                    Total_Units = houseitem.Total_Units,
                    Owner_Firstname = houseitem.Owner_LastName,
                    Owner_LastName = houseitem.Owner_LastName,
                    Owner_id_Number = houseitem.Owner_id_Number,
                    House_Name = houseitem.House_Name,
                    CreatedBy = houseitem.CreatedBy,
                    DateCreated = houseitem.DateCreated,
                    Country = houseitem.Country,
                    Area = houseitem.Area,
                    CreatorEmail = houseitem.CreatorEmail,
                    CreatorNames = houseitem.CreatorNames,
                };
                houses.Add(houselistvm);
            }

            if (user.Is_Admin)
            {
                var response = AdminHouseList().Result.Body;
                return new BaseResponse { Code = "200", SuccessMessage = "Successful", Body = response };
            }

            else
            {
                return new BaseResponse { Code = "200", SuccessMessage = "Successful", Body = houses };
            }
        }


        public async Task<BaseResponse> AddAdminContacts(AdminContctsViewModel vm)
        {
            var user = LoggedInUser().Result;
            if (user.Is_Agent)
            {
                if (vm.AdminEmail == "")
                {
                    return new BaseResponse
                    {
                        Code = "120",
                        ErrorMessage = "Admin email cannot be empty"

                    };
                }
                if (vm.Creator == "")
                {
                    return new BaseResponse { Code = "120", ErrorMessage = "Email address cannot be empty" };
                }

                if (vm.AdminPhoneNumber == "")
                {
                    return new BaseResponse { Code = "122", ErrorMessage = "Phone Number cannot be empty" };
                }
                if (user.Is_Agent)
                {
                    var admincontactsmodel = new AdminContacts
                    {
                        AdminEmail = vm.AdminEmail,
                        Creator = user.Email,
                        AdminPhoneNumber = vm.AdminPhoneNumber,
                    };
                    await _context.AddAsync(admincontactsmodel);
                    await _context.SaveChangesAsync();
                    return new BaseResponse { Code = "200", SuccessMessage = "Contact Details Added successfully" };
                }
            }
            else
            {
                return new BaseResponse { Code = "124", ErrorMessage = "you dont have the permision to do This" };
            }
            return new BaseResponse { };
        }


        public async Task<BaseResponse> GetHoousesByLocation(string GetHoousesByLocation)
        {
            var houselocation = await _context.House_Registration
                .Where(x => x.House_Location == GetHoousesByLocation)
                .ToListAsync();
            if (houselocation.Count == 0)
            {
                return new BaseResponse { Code = "000", ErrorMessage = "Nothing to show " };
            }

            return new BaseResponse
            {
                Code = "200",
                SuccessMessage = "Query Run successfully",
                Body = houselocation
            };
        }

        public async Task<BaseResponse> GetHousesBy_OwnerIdNumber(int OwnerId)
        {
            var gethouses = await _context.House_Registration
                .Where(x => x.Owner_id_Number == OwnerId)
                .ToListAsync();
            try
            {
                if (gethouses.Count == 0)
                {
                    return new BaseResponse
                    {
                        Code = "420",
                        ErrorMessage = "The house does not exist"
                    };
                }
                return new BaseResponse
                {
                    Code = "200",
                    SuccessMessage = "Successful",
                    Body = gethouses
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "000", ErrorMessage = ex.Message };
            }
        }


        public async Task<BaseResponse> CreateHouseUser(HouseUsersViewModel vm)
        {
            var user = LoggedInUser().Result;
            var house = await _context.House_Registration.Where(h => h.HouseiD == vm.HouseID).FirstOrDefaultAsync();
            try
            {
                var createnewnusr = new RegisterViewModel()
                {
                    FirstName = vm.FirstName,
                    LasstName = vm.LasstName,
                    BirthDate = "00/00/00",
                    PhoneNumber = vm.PhoneNumber,
                    Email = vm.Email,
                    Salutation = vm.PhoneNumber,
                    IdNumber = vm.IdNumber,
                    Password = "Password@1234",
                    RetypePassword = "Password@1234",
                    Gender = "N/A",
                    IsHouseUsers = true,
                };
                var response = await _registrationServices.UserRegistration(createnewnusr);
                var specifichouse = await _context.House_Registration
                    .Where(x => x.HouseiD == vm.HouseID).FirstOrDefaultAsync();
                if (response.Code == "200" || response.Code == "100")
                {
                    var newhouseUser = new HouseUsers()
                    {
                        FirstName = vm.FirstName,
                        LasstName = vm.LasstName,
                        BirthDate = vm.BirthDate,
                        PhoneNumber = vm.PhoneNumber,
                        Email = vm.Email,
                        Salutation = vm.Salutation,
                        IdNumber = vm.IdNumber,
                        HouseID = vm.HouseID,
                        Password = "Password@1234",
                        RetypePassword = "Password@1234",
                        //CreatorId = Convert.ToInt32(user.Id),
                        Creatormail = user.Email,
                        HouseName = house.House_Name,
                        AccountActivated = false,
                    };
                    var check_house_user_exist = await _context.HouseUsers.Where(x => x.Email == vm.Email).FirstOrDefaultAsync();
                    if (check_house_user_exist != null)
                    {
                        return new BaseResponse { Code = "150", ErrorMessage = "The house user already exists" };
                    }
                    await _context.AddAsync(newhouseUser);
                    await _context.SaveChangesAsync();
                    return new BaseResponse { Code = "200", SuccessMessage = "House user created successfully " };
                }
                else
                {
                    return new BaseResponse { Code = "380", ErrorMessage = "House User could not be created" };
                }
            }

            catch (Exception ex)
            {
                return new BaseResponse { Code = "245", ErrorMessage = ex.Message };

            }
        }

        public async Task<BaseResponse> gethouseById(int houseid)

        {
            var houses = await _context.House_Registration.Where(x => x.HouseiD == houseid).FirstOrDefaultAsync();

            return new BaseResponse { Code = "200", SuccessMessage = houses.House_Name };
        }

        public async Task<BaseResponse> GetHouseUser(int houseid)
        {
            try
            {
                //var user = LoggedInUser().Result;
                if (houseid == 0)
                {
                    return new BaseResponse { Code = "129", ErrorMessage = "Kindly choose a house " };
                }
                else
                {
                    var houseUsers = await _context.HouseUsers.Where(u => u.HouseID == houseid).OrderByDescending(t => t.DateCreated).ToListAsync();
                    var houses = await _context.House_Registration.Where(x => x.HouseiD == houseid).FirstOrDefaultAsync();
                    List<houseUserresponsevm> houseusersresponse = new List<houseUserresponsevm>();
                    foreach (var houseUser in houseUsers)
                    {
                        var house_user = new houseUserresponsevm
                        {
                            FirstName = houseUser.FirstName,
                            LasstName = houseUser.LasstName,
                            Email = houseUser.Email,
                            HouseName = houses.House_Name,
                            PhoneNumber = houseUser.PhoneNumber,
                            DateCreated = houseUser.DateCreated
                        };
                        houseusersresponse.Add(house_user);
                    }
                    return new BaseResponse { Code = "200", Body = houseusersresponse };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "130", ErrorMessage = ex.Message };
            }
        }

        public async Task<BaseResponse> Get_HouseUsers_Houses()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var user = LoggedInUser().Result;

                    if (user.IsHouseUsers)
                    {
                        var houseuser = await scopedcontext.HouseUsers.Where(t => t.Email == user.Email).FirstOrDefaultAsync();
                        if (houseuser == null)
                        {
                            return new BaseResponse { Code = "150", ErrorMessage = "Tenant does not exist" };
                        }
                        var house = await scopedcontext.House_Registration.Where(h => h.HouseiD == houseuser.HouseID).ToListAsync();
                        if (houseuser == null)
                        {
                            return new BaseResponse { Code = "170", ErrorMessage = "House does not exist" };
                        }
                        if (user.Is_Agent || user.Is_Landlord || user.Is_CareTaker)
                        {
                            _logger.LogInformation($"houses reached :{house}  {DateTime.Now}");
                            return new BaseResponse { Code = "200", SuccessMessage = "Queried successfully", Body = house };
                        }
                        else
                        {
                            return new BaseResponse { Code = "180", ErrorMessage = "Incomplete setup! Contact the admin for a complete set up " };
                        }
                    }
                    return new BaseResponse();
                }

            }
            catch (Exception ex)
            {

                return new BaseResponse { Code = "130", ErrorMessage = ex.Message };
            }



        }

        public async Task<BaseResponse> AdminHouseList()
        {

            var Adminhouselists = await _context.House_Registration.OrderByDescending(x => x.DateCreated).ToListAsync();

            List<HouseListsViewModel> Adminhouses = new List<HouseListsViewModel>();
            foreach (var houseitem in Adminhouselists)
            {

                var houselistvm = new HouseListsViewModel
                {

                    House_Location = houseitem.House_Location,
                    HouseiD = houseitem.HouseiD,
                    Total_Units = houseitem.Total_Units,
                    Owner_Firstname = houseitem.Owner_LastName,
                    Owner_LastName = houseitem.Owner_LastName,
                    Owner_id_Number = houseitem.Owner_id_Number,
                    House_Name = houseitem.House_Name,
                    CreatedBy = houseitem.CreatedBy,
                    DateCreated = houseitem.DateCreated,
                    Country = houseitem.Country,
                    Area = houseitem.Area,
                    CreatorEmail = houseitem.CreatorEmail,
                    CreatorNames = houseitem.CreatorNames,
                };

                Adminhouses.Add(houselistvm);
            }

            return new BaseResponse { Code = "200", Body = Adminhouses };
        }

        public async Task<AggreementResponse> CreateAggreement(aggreementvm vm)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateAsyncScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();


                    var tenantexist = await scopedcontext.TenantClass.Where(a => a.Email == vm.TenantEmail).FirstOrDefaultAsync();

                    if (tenantexist == null)
                    {
                        return new AggreementResponse("190", "tenant does not exist", null);
                    }

                    var houseexist = await scopedcontext.House_Registration.Where(a => a.HouseiD == tenantexist.HouseiD).FirstOrDefaultAsync();

                    if (houseexist == null)
                    {
                        return new AggreementResponse("160", "house does not exist", null);
                    }
                    var loggeininuser = _iloggedInServices.LoggedInUser().Result;
                    var newaggrrement = new Aggrement
                    {
                        CreatedBy = loggeininuser.Email,
                        HouseID = vm.HouseID,
                        LandlordName = vm.LandlordName,
                        Agent = vm.Agent,
                        AggreeToAggreement = vm.AggreeToAggreement,
                        LeastStartDate = Convert.ToDateTime(vm.LeastStartDate),
                        LeastEndDateDate = Convert.ToDateTime(vm.LeastEndDateDate),
                        RentAmount = vm.RentAmount,
                        MaintainceAndRepairDeposit = vm.MaintainceAndRepairDeposit,
                        RentIncreasePeriod = vm.RentIncreasePeriod,
                        RentDepositAmount = vm.RentDepositAmount,
                        Rentincreasepercentage = vm.Rentincreasepercentage,
                        Renincreaseflatrate = vm.Renincreaseflatrate,
                        Serviceffeedeposit = vm.Serviceffeedeposit,
                        AnyOtherTerms = vm.AnyOtherTerms,
                        HouseLocation = vm.HouseLocation,
                        TenantName = tenantexist.FirstName + " " + tenantexist.LastName,
                        TenantEmail = tenantexist.Email,
                        AggreementStatus = false,
                        TenantId = tenantexist.RenteeId,
                        HouseAddress = "House Name: " + houseexist.House_Name + ", House Location: " + houseexist.House_Location + ",Door Number " + tenantexist.Appartment_DoorNumber,
                        Landlordphone = tenantexist.Agent_PhoneNumber,
                        Tenantphone = tenantexist.Rentee_PhoneNumber
                    };

                    //check tenant already has aggrement

                    var aggreementexists = await scopedcontext.Aggrement.Where(y => y.TenantEmail == vm.TenantEmail).FirstOrDefaultAsync();

                    if (aggreementexists != null)
                    {
                        return new AggreementResponse("120", $" {tenantexist.FirstName} {tenantexist.LastName}  already has a tenancy aggreement ", null);
                    }
                    if (vm.AggreeToAggreement)
                    {
                        newaggrrement.AggreeToAggreement = true;
                        newaggrrement.EnforceAggreement = true;
                    }
                    await scopedcontext.AddAsync(newaggrrement);
                    var res = await scopedcontext.SaveChangesAsync();
                    return new AggreementResponse("200", "Aggreement created successfully ", null);
                }
            }
            catch (Exception ex)
            {
                return new AggreementResponse("140", ex.Message, null);
            }
        }

        public async Task<AggreementResponse> AddSection(Sectionsvm vm)
        {
            var newsection = new Sections
            {
                IsTrue = vm.IsTrue,
                SectionName = vm.SectionName
            };
            await _context.AddAsync(newsection);
            await _context.SaveChangesAsync();
            return new AggreementResponse("200", "successfully updated section", null);

        }

        public async Task AggreementSections(AggrementSections aggreementsection)
        {
            var addaggreementsection = new AggrementSections
            {
                show_LandlordName = aggreementsection.show_LandlordName,
                LandlordName = aggreementsection.LandlordName,
                show_TenantNmae = aggreementsection.show_TenantNmae,
                TenantNmae = aggreementsection.TenantNmae,
                show_AgentName = aggreementsection.show_AgentName,
                AgentName = aggreementsection.AgentName,
                show_HouseName = aggreementsection.show_HouseName,
                HouseName = aggreementsection.HouseName,
                show_HouseLocation = aggreementsection.show_HouseLocation,
                HouseLocation = aggreementsection.HouseLocation,
                show_SecurityDeposit = aggreementsection.show_SecurityDeposit,
                SecurityDeposit = aggreementsection.SecurityDeposit,
                show_ServiceFee = aggreementsection.show_ServiceFee,
                ServiceFee = aggreementsection.ServiceFee,
                show_Maintainance_and_Repairs = aggreementsection.show_Maintainance_and_Repairs,
                Maintainance_and_Repairs = aggreementsection.Maintainance_and_Repairs,
                show_Rent_Increased_After_in_years = aggreementsection.show_Rent_Increased_After_in_years,
                Rent_Increased_After_in_years = aggreementsection.Rent_Increased_After_in_years,
                show_Increasepercentage = aggreementsection.show_Increasepercentage,
                Increasepercentage = aggreementsection.Increasepercentage,
                show_Increase_flat_rate = aggreementsection.show_Increase_flat_rate,
                Increase_flat_rate = aggreementsection.Increase_flat_rate,
                show_Other_Aggreement = aggreementsection.show_Other_Aggreement,
                Other_Aggreements = aggreementsection.Other_Aggreements
            };
            _context.Update(addaggreementsection);
            await _context.SaveChangesAsync();
        }
        public async Task<AggreementResponse> GetAllAggreementSections()
        {
            try
            {
                using (var scoped = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scoped.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var Allaggreementsections = await scopedcontext.Sections.ToListAsync();
                    if (Allaggreementsections == null)
                    {
                        return new AggreementResponse("110", "The sections do not exist", null);
                    }
                    return new AggreementResponse("200", "Queried successfully", Allaggreementsections);
                }
            }
            catch (Exception ex)
            {
                return new AggreementResponse("180", ex.Message, null);
            }
        }

        public async Task<AggreementResponse> SelectAggeementSections(int aggreementID, int aggreeementSectionID)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var AggreementExists = await scopedcontext.Aggrement.Where(x => x.AggreementID == aggreementID).FirstOrDefaultAsync();
                    if (AggreementExists == null)
                    {
                        return new AggreementResponse("190", "The aggreement does not exist", null);
                    }
                    var sectionExists = await scopedcontext.Sections.Where(x => x.AggreementSectiondID == aggreeementSectionID).FirstOrDefaultAsync();
                    if (sectionExists == null)
                    {
                        return new AggreementResponse("190", "The aggreement section  does not exist", null);
                    }
                    var AddAggreementSection = new SectionMapper
                    {
                        AggreemenID = AggreementExists.AggreementID,
                        AggreementSectionID = sectionExists.AggreementSectiondID
                    };
                    scopedcontext.Update(AddAggreementSection);
                    await scopedcontext.SaveChangesAsync();
                    return new AggreementResponse("200", "Successfully queried", AddAggreementSection);
                }

            }
            catch (Exception ex)
            {
                return new AggreementResponse("190", ex.Message, null);
            }
        }

        public async Task<classicaggreementresponse> GetAggementSections(int aggreemeniD)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var aggreementExists = await scopedcontext.Aggrement.Where(x => x.AggreementID == aggreemeniD).FirstOrDefaultAsync();
                    if (aggreementExists == null)
                    {
                        return new classicaggreementresponse("120", "aggreement not found", "NOthing to show here", null);
                    }
                    var allaggreementsections = await scopedcontext
                        .SectionMapper.Where(s => s
                        .AggreemenID == aggreementExists.AggreementID).ToListAsync();
                    List<Sections> sectionsofAggreements = new List<Sections>();
                    foreach (var eachsection in allaggreementsections)
                    {
                        var sectionnameexists = await scopedcontext.Sections
                           .Where(e => e.AggreementSectiondID == eachsection.AggreementSectionID).FirstOrDefaultAsync();
                        var sectionsvmvalue = new Sections
                        {
                            SectionName = sectionnameexists.SectionName,
                            IsTrue = sectionnameexists.IsTrue
                        };
                        sectionsofAggreements.Add(sectionsvmvalue);
                    }
                    var houseexists = await scopedcontext.House_Registration.Where(h => h.HouseiD == aggreementExists.HouseID).FirstOrDefaultAsync();
                    if (houseexists == null)
                    {
                        return new classicaggreementresponse("127", "House not found", "NOthing to show here", null);
                    }
                    return new classicaggreementresponse("200", "Queried successfully", houseexists.House_Name, sectionsofAggreements);

                }
            }
            catch (Exception ex)
            {
                return new classicaggreementresponse("120", ex.Message, "Nothing  to show here ", null);
            }
        }

        public async Task<classicaggreementresponse> GetAggementSectionsByHouseID(int HouseID)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var houseexisted = await scopedcontext.House_Registration.Where(x => x.HouseiD == HouseID).FirstOrDefaultAsync();
                    if (houseexisted == null)
                    {
                        return new classicaggreementresponse("120", "House not found", "NOthing to show here", null);
                    }
                    var aggreementexists = await scopedcontext.Aggrement.Where(a => a.HouseID == houseexisted.HouseiD).ToListAsync();
                    if (aggreementexists == null)
                    {
                        return new classicaggreementresponse("120", "aggreement not found", "NOthing to show here", null);
                    }
                    return new classicaggreementresponse("200", "Aggreement queries successfully", $"agreement on house {houseexisted.House_Name}", aggreementexists);
                }
            }
            catch (Exception ex)
            {
                return new classicaggreementresponse("120", ex.Message, "Nothing  to show here ", null);
            }
        }
        public async Task<AggreementResponse> GetggreementByHouseID(int houseid)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var aggreeementexists = await scopedcontext.Aggrement.Where(a => a.HouseID == houseid).FirstOrDefaultAsync();
                    if (aggreeementexists == null)
                    {
                        return new AggreementResponse("160", "The aggreement does not exist", null);
                    }
                    return new AggreementResponse("200", "Successfully queried", aggreeementexists);
                }
            }

            catch (Exception ex)
            {
                return new AggreementResponse("120", ex.Message, null);
            }
        }

        public async Task<object> GethouseById(int houseid)
        {
            var houseexists = await _context.House_Registration.Where(h => h.HouseiD == houseid).FirstOrDefaultAsync();
            if (houseexists == null)
            {
                return null;
            }
            return houseexists;
        }
        public async Task<BaseResponse> GetAggreementByTenantId(int tenantid)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    //tenant exists

                    var tenantexists = await scopedcontext.TenantClass.Where(h => h.RenteeId == tenantid).FirstOrDefaultAsync();
                    if (tenantexists == null)
                    {
                        return new BaseResponse { ErrorMessage = "tenant does not exist" };
                    }

                    //aggreemnt exists 

                    var aggreementexists = await scopedcontext.Aggrement.Where(h => h.TenantId == tenantid).FirstOrDefaultAsync();
                    if (aggreementexists == null)
                    {
                        return new BaseResponse { ErrorMessage = "aggreement does not exist" };
                    }

                    return new BaseResponse { Code = "200", SuccessMessage = "successfully queried ", Body = aggreementexists };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "150", ErrorMessage = ex.Message };
            }
        }

        public async Task<BaseResponse> GetUnoccupiedhouseunits(string housename)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var un_occupied_house_units = await scopedcontext.HouseUnitsStatus.Where(u => u.HouseName == housename && !u.Occupied).ToListAsync();
                    if (un_occupied_house_units == null)
                    {
                        return new BaseResponse();
                    }
                    return new BaseResponse { Code = "200", SuccessMessage = "Successfully queried", Body = un_occupied_house_units };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "130", ErrorMessage = ex.Message };
            }
        }

        public async Task<Housing_Profile_Response> Get_House_Details_By_Id(int house_id)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopecontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    var house_exists = await scopecontext.House_Registration.Where(y => y.HouseiD == house_id).FirstOrDefaultAsync();

                    if (house_exists == null)
                    {
                        return new Housing_Profile_Response { Code = "190", ErrorMessage = "The house does not exists" };
                    }

                    var total_occupeid_units = scopecontext.HouseUnitsStatus.Where(y => y.Occupied == true && y.HouseName == house_exists.House_Name).Count();
                    var un_occpuied_units = scopecontext.HouseUnitsStatus.Where(y => y.Occupied == false && y.HouseName == house_exists.House_Name).Count();
                    var total_house_units = scopecontext.HouseUnitsStatus.Where(y => y.HouseName == house_exists.House_Name).Count();
                    var total_expected_service_fee = await scopecontext.TenantClass.Where(y => y.HouseiD == house_exists.HouseiD).SumAsync(y => y.ServicesFees);
                    var total_monthly_rent_amount = await scopecontext.TenantClass.Where(y => y.HouseiD == house_id).ToListAsync();

                    var total_Tenants = scopecontext.TenantClass.Where(y => y.HouseiD == house_id).Count();
                    var monthly_rent_totals = total_monthly_rent_amount.Sum(y => y.House_Rent);
                    var total_montly_expected_amount = total_expected_service_fee + monthly_rent_totals;

                    return new Housing_Profile_Response
                    {
                        Code = "200",
                        Body = house_exists,
                        Occupied_Units = total_occupeid_units,
                        Un_Occupied_Units = un_occpuied_units,
                        Total_Rent = monthly_rent_totals,
                        Total_Units = total_house_units
                                                              ,
                        Total_Expected_Service = (float)total_expected_service_fee,
                        Total_Amounts = total_montly_expected_amount,
                        Total_Tenants = total_Tenants

                    };

                }

            }
            catch (Exception ex)
            {
                return new Housing_Profile_Response { Code = "140", ErrorMessage = ex.Message };
            }
        }

        public async Task<BaseResponse> Change_House_unit_Status(string house_name, int door_number, string unit_status)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopecontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    var house_units_exists = await scopecontext.HouseUnitsStatus.Where(y => y.HouseName == house_name && y.DoorNumber == door_number).FirstOrDefaultAsync();

                    if (house_units_exists == null)
                        return new BaseResponse { Code = "190", ErrorMessage = "An error occured, kindly make sure the details provided are correct ad try again" };
                    if (unit_status == "Occupied")
                    {

                        if (house_units_exists.Occupied)
                        {
                            return new BaseResponse { Code = "140", ErrorMessage = "The house status is already occupied" };
                        }
                        else
                        {
                            house_units_exists.Occupied = true;
                            scopecontext.Update(house_name);
                            await scopecontext.SaveChangesAsync();
                            return new BaseResponse { Code = "200", SuccessMessage = "House status updated to ccupied successfully" };
                        }
                    }
                    else if (unit_status == "un_Occupied")
                    {
                        if (!house_units_exists.Occupied)
                        {
                            return new BaseResponse { Code = "040", ErrorMessage = "The house  is already vacant" };
                        }
                        else
                        {
                            house_units_exists.Occupied = false;
                            scopecontext.Update(house_name);
                            await scopecontext.SaveChangesAsync();
                            return new BaseResponse { Code = "200", SuccessMessage = "The house unit is now vacant" };
                        }

                    }
                    return new BaseResponse();
                }

            }
            catch (Exception ex)
            {

                return new BaseResponse { Code = "140", ErrorMessage = ex.Message };
            }
        }

    }
}




