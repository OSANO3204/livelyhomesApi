using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Extras;
using HousingProject.Core.Models.Professionals;
using HousingProject.Core.ViewModel;
using HousingProject.Core.ViewModel.Professionalsvm;
using HousingProject.Infrastructure.ExtraFunctions.GenerateWorkId;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
using HousingProject.Infrastructure.Interfaces.IProfessionalsServices;
using HousingProject.Infrastructure.Response;
using HousingProject.Infrastructure.Response.VotesResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.CRUDServices.ProfessionalsServices
{
    public class ProfessionalServices : IProfessionalsServices
    {
        private readonly HousingProjectContext _context;
        private readonly ILoggedIn _loggedIn;
        private readonly IServiceScopeFactory _servicescopefactory;
        private readonly IGenerateIdService _generateIdService;
        private readonly ILoggedIn _logged_in_user;
        private readonly ILogger<ProfessionalServices> _logger;
        public ProfessionalServices(
            ILoggedIn loggedIn,
            HousingProjectContext context,
            IGenerateIdService generateIdService,
            IServiceScopeFactory servicescopefactory,
            ILoggedIn logged_in_user,
            ILogger<ProfessionalServices> logger
            )
        {
            _loggedIn = loggedIn;
            _context = context;
            _generateIdService = generateIdService;
            _servicescopefactory = servicescopefactory;
            _logged_in_user = logged_in_user;
            _logger = logger;
        }

        public async Task<BaseResponse> Createprofessonal(Professionalsvm vm)
        {
            var loggedid_in=_loggedIn.LoggedInUser().Result;
            try
            {
                var user =  _logged_in_user.LoggedInUser().Result;
                var workid = _generateIdService.GenerateWorkId().Result.SuccessMessage + Adding_Number().Result;
                var newprofessional = new RegisterProfessional
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    ProfessionName = vm.ProfessionName,
                    County = vm.County,
                    OperationArea = vm.OperationArea,
                    PhoneNumber = vm.PhoneNumber,
                    WorkDescription = vm.WorkDescription,
                    Salutation = vm.Salutation,
                    Email = vm.Email,
                    JobNumber = workid,
                    User_Id = Convert.ToString(loggedid_in.Id)
                };
                //ee79fa63-e86d-4cfd-80e9-c256fa0b2f9d
                await _context.AddAsync(newprofessional);
                await _context.SaveChangesAsync();

                return new BaseResponse { Code = "200", SuccessMessage = "Professional registered successfully" };
            }

            catch (Exception ex)
            {

                return new BaseResponse { Code = "170", ErrorMessage = ex.Message };
            }

        }


        public async Task<BaseResponse> GetTechnicianByName(string ProfesionName)
        {

            try
            {

                if (ProfesionName == "")
                {
                    return new BaseResponse { Code = "145", ErrorMessage = "Wrong technician name" };

                }

                var technicians = await _context.RegisterProfessional.Where(x => x.ProfessionName == ProfesionName).OrderByDescending(x => x.DateCreated).ToListAsync();

                if (technicians == null)
                {

                    return new BaseResponse { Code = "140", ErrorMessage = "Technicians cannot be empty" };
                }

                return new BaseResponse { Code = "200", SuccessMessage = "Successfull", Body = technicians };

            }

            catch (Exception ex)
            {

                return new BaseResponse { Code = "230", ErrorMessage = ex.Message.ToString() };

            }

        }

        public async Task<BaseResponse> GetProfessionalById(int id)
        {

            if (id == 0)
            {
                return new BaseResponse { Code = "140", ErrorMessage = "Kindly use the correct id for this person" };

            }

            try
            {

                var technician = await _context.RegisterProfessional.Where(x => x.ProfessionalId == id).FirstOrDefaultAsync();

                if (technician == null)
                {

                    return new BaseResponse { Code = "234", ErrorMessage = "The technician does not exist" };
                }

                return new BaseResponse { Code = "200", Body = technician };

            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "130", ErrorMessage = ex.Message };
            }
        }


        public async Task<BaseResponse> GetProfessionalByEmail()
        {


            var loggeinuserEmail = _loggedIn.LoggedInUser().Result.Email;
            try
            {

                var profesionalexists = await _context.RegisterProfessional.Where(p => p.Email == loggeinuserEmail).FirstOrDefaultAsync();

                if (profesionalexists == null)
                {

                    return new BaseResponse { Code = "345", ErrorMessage = "Professional does not exist" };
                }
                return new BaseResponse { Code = "200", SuccessMessage = "Successful", Body = profesionalexists };

            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "345", ErrorMessage = ex.ToString() };
            }
        }


        // total upvotes
        public async Task<VotesResponse> Update_UpVotes(int userid)
        {
            try
            {
                using (var scope = _servicescopefactory.CreateScope())
                {
                    //check if user exists
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var professionalexists = await scopedcontext.RegisterProfessional.Where(y => y.ProfessionalId == userid).FirstOrDefaultAsync();
                    if (professionalexists == null)
                    {
                        return new VotesResponse("140", "User does not exist", 0, 0, 0, 0);
                    };

                    //update user with a vote
                    professionalexists.Upvotes += 1;
                    professionalexists.TotalVotes += 1;

                    var rateupdate = Math.Round((double)((professionalexists.Upvotes / professionalexists.TotalVotes) * 5), 1);
                    scopedcontext.Update(professionalexists);
                    await scopedcontext.SaveChangesAsync();
                    return new VotesResponse("200", "User upvotes  queried successfully", professionalexists.TotalVotes, professionalexists.Upvotes, professionalexists.Downvotes, rateupdate);
                }
            }
            catch (Exception ex)
            {
                return new VotesResponse("140", ex.Message, 0, 0, 0, 0);
            }
        }


        //tottal downotes
        public async Task<VotesResponse> Update_DownVotes(int userid)
        {
            try
            {
                using (var scope = _servicescopefactory.CreateScope())
                {
                    //check if user exists
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var professionalexists = await scopedcontext.RegisterProfessional.Where(y => y.ProfessionalId == userid).FirstOrDefaultAsync();
                    if (professionalexists == null)
                    {
                        return new VotesResponse("140", "User does not exist", 0, 0, 0, 0);
                    };

                    //update user with a vote
                    professionalexists.Downvotes += 1;
                    professionalexists.TotalVotes += 1;
                    var rateupdate = Math.Round((double)((professionalexists.Upvotes / professionalexists.TotalVotes) * 5), 1);
                    scopedcontext.Update(professionalexists);
                    await scopedcontext.SaveChangesAsync();
                    return new VotesResponse("200", "User downvotes   updated successfully", professionalexists.TotalVotes, professionalexists.Upvotes, professionalexists.Downvotes, rateupdate);
                }
            }
            catch (Exception ex)
            {
                return new VotesResponse("140", ex.Message, 0, 0, 0, 0);
            }
        }

        public async Task<VotesResponse> Userrating(int userid)
        {
            using (var scope = _servicescopefactory.CreateScope())
            {
                double rateupdate = 0;
                //check if user exists
                var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                var professionalexists = await scopedcontext.RegisterProfessional.Where(y => y.ProfessionalId == userid).FirstOrDefaultAsync();
                if (professionalexists == null)
                {
                    return new VotesResponse("140", "User does not exist", 0, 0, 0, 0);
                };
                if (professionalexists.TotalVotes == 0)
                {
                 return new VotesResponse("200", "User rating  queried successfully", professionalexists.TotalVotes, professionalexists.Upvotes, professionalexists.Downvotes, rateupdate);
                }
                rateupdate = Math.Round((double)((professionalexists.Upvotes / professionalexists.TotalVotes) * 5), 1);
                return new VotesResponse("200", "User rating   queried successfully", professionalexists.TotalVotes, professionalexists.Upvotes, professionalexists.Downvotes, rateupdate);
            }
        }

        public async Task<BaseResponse> Get_User_Profession(string user_id)
        {
            try
            {
               
                using (var scope = _servicescopefactory.CreateScope())
                {

                    var user = _loggedIn.LoggedInUser().Result;
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var professionalexists = await scopedcontext.RegisterProfessional.Where(y => y.User_Id == user_id ).OrderByDescending(y=>y.DateCreated).ToListAsync();
                    if (professionalexists == null)
                    {
                        return new BaseResponse { Code = "140", ErrorMessage = "User does not exist" };
                    };
                    return new BaseResponse { Code = "200", SuccessMessage = "Queried successfully" , Body=professionalexists};
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "180", ErrorMessage = ex.Message };
            }

        }

        public async Task<professional_profile_Response>  Get_technician_profile_with_job( string job_number)
        {

            try
            {
                 
                using (var scope = _servicescopefactory.CreateScope()) {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var profile_body = await scopedcontext.RegisterProfessional
                        .Where(y => y.JobNumber == job_number).FirstOrDefaultAsync();


                    if (profile_body == null)
                    {

                        return new professional_profile_Response { Code = "170", ErrorMessage = "Object not found" };
                    }

                    var logged_in_user = await _loggedIn.LoggedInUser();
                    var rating_response =  Userrating(profile_body.ProfessionalId).Result;
                    var rating = rating_response.UserRating;

                    return new professional_profile_Response
                    {
                        Code = "200",
                        SuccessMessage = "Queried successfully",
                        Body = profile_body,
                        Rating = rating,


                    };
                }
            }
           catch(Exception ex)
            {
                return new professional_profile_Response { Code = "180", ErrorMessage = ex.Message };
            }
        }


        public async Task<BaseResponse> AddingRequest_to_Worker(add_request__vm vm)
        {

            try
            {
                using (var scope = _servicescopefactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var loggeinuserEmail = _loggedIn.LoggedInUser().Result.Email;

                    var worker_object = await scopedcontext.RegisterProfessional.Where(y => y.JobNumber == vm.Job_Number).FirstOrDefaultAsync();

                    if (worker_object == null)
                    { return new BaseResponse { Code = "worker doesnt exists" }; }

                    var new_request = new Add_User_Request
                    {
                        Description = vm.Description,
                        Reason = vm.reason,
                        Job_Number = vm.Job_Number,
                        Worker_Email = vm.worker_Email,
                        Phone_Number=vm.Phone_Number,
                        Names=vm.Names,
                        RequesterEmail= loggeinuserEmail,
                        Worker_phone=worker_object.PhoneNumber


                    };

                    await scopedcontext.AddAsync(new_request);
                    await scopedcontext.SaveChangesAsync();
                    return new BaseResponse
                    {
                        Code = "200",
                        SuccessMessage = "Your request is sent successfully , " +
                        "the agent will contact you shortly"
                    };

                }


            }
            catch (Exception ex)
            {

                return new BaseResponse { Code = "340", ErrorMessage = ex.Message };
            }
        }


            public async Task<BaseResponse> Get_Technician_Requests(string worker_email)
            {

                try
                {
                    using (var scope = _servicescopefactory.CreateScope())
                    {

                        var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    var allrequests = await scopedcontext.Add_User_Request.Where(y => y.Worker_Email == worker_email).OrderByDescending(y=>y.CreatedOn).ToListAsync();
                    if (allrequests == null)
                    {
                        return new BaseResponse { Code = "170", ErrorMessage = "No requests  found" };
                    }

                      
                        return new BaseResponse
                        {
                            Code = "200",
                            SuccessMessage = "Queried successfully",
                            Body=allrequests
                        };

                    }


                }
                catch (Exception ex)
                {

                    return new BaseResponse { Code = "340", ErrorMessage = ex.Message };
                }
            }


        public async Task<BaseResponse> Get_request_by_Job_Number(string  job_number)
        {

            try
            {
                using (var scope = _servicescopefactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    var allrequests = await scopedcontext.Add_User_Request.Where(y => y.Job_Number == job_number).OrderByDescending(y=>y.CreatedOn).ToListAsync();
                    if (allrequests == null)
                    {
                        return new BaseResponse { Code = "170", ErrorMessage = "No requests  found" };
                    }

                    return new BaseResponse
                    {
                        Code = "200",
                        SuccessMessage = "Queried successfully",
                        Body = allrequests
                    };

                }


            }
            catch (Exception ex)
            {

                return new BaseResponse { Code = "340", ErrorMessage = ex.Message };
            }
        }
        public async Task<BaseResponse> Close_Request(int request_id)
        {
            try
            {
                using(var scope = _servicescopefactory.CreateScope())
                {
                    var scopedocntext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var request_exists = await scopedocntext.Add_User_Request.Where(y => y.request_id == request_id).FirstOrDefaultAsync();

                    if (request_exists == null)
                    {
                        return new BaseResponse { Code = "140", ErrorMessage = "This request does not exist" };
                    }
                    request_exists.Is_Closed = true;
                    scopedocntext.Update(request_exists);
                    await scopedocntext.SaveChangesAsync();

                    return new BaseResponse { Code = "200", SuccessMessage = "The request  has been closed successfully" };

                }

            }
            catch(Exception ex){
                return new BaseResponse { Code = "130", ErrorMessage = ex.Message };


            }
        }

        public async Task<BaseResponse> Add_Services(string service_added, string job_number)
        {

            try
            {
                using (var scope = _servicescopefactory.CreateScope())
                {

                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    var new_service = new Add_Services
                    {
                        Service = service_added,
                       
                        Job_Number=job_number
                    };

                    await scopedcontext.AddAsync(new_service);
                    await scopedcontext.SaveChangesAsync();
                    return new BaseResponse
                    {
                        Code = "200",
                        SuccessMessage = "You added a service successfully"
                    };

                }


            }
            catch (Exception ex)
            {

                return new BaseResponse { Code = "340", ErrorMessage = ex.Message };
            }
        }

        public async Task<BaseResponse> Get_Services_By_Job_Number(string job_number)
        {

            try
            {
                using (var scope = _servicescopefactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var all_services_jobs = await scopedcontext.Add_Services.Where(y => y.Job_Number == job_number).OrderByDescending(y=>y.DateCreated).ToListAsync();

                    if (all_services_jobs == null)
                    {
                        return new BaseResponse { Code = "200", ErrorMessage = "The services don't exist" };
                    }
                    return new BaseResponse
                    {
                        Code = "200",
                        SuccessMessage = "successfully queried ",
                        Body = all_services_jobs
                    };

                }
            }
            catch (Exception ex)
            {

                return new BaseResponse { Code = "340", ErrorMessage = ex.Message };
            }
        }
        public async Task<int> Adding_Number()
        {
            try
            {
                using (var scope = _servicescopefactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();


                    var check_last_number = await scopedcontext.Number_Generator.Where(y => y.Generated_Number > 0).OrderByDescending(u => u.DateUpdated).LastOrDefaultAsync();

                    if (check_last_number == null)
                    {

                        var new_number = new Number_Generator
                        {
                            Generated_Number = 1

                        };

                        await scopedcontext.AddAsync(new_number);
                        await scopedcontext.SaveChangesAsync();
                        _logger.LogInformation("Number added for the first time");
                        return new_number.Generated_Number;
                    }
                    else
                    {
                        var number_update = 1;
                        check_last_number.Generated_Number = check_last_number.Generated_Number + number_update;
                        scopedcontext.Update(check_last_number);
                        await scopedcontext.SaveChangesAsync();

                        _logger.LogInformation("Successfully done");

                        return check_last_number.Generated_Number;
                    }
                }

            }
            catch (Exception ex)
            {
                return ex.Data.Count;

            }
        }

        public async Task<BaseResponse> My_Repair_Requests()
        {

            var usermail = _loggedIn.LoggedInUser().Result.Email; 

            try
            {
                using(var scope = _servicescopefactory.CreateScope())
                {
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();

                    var all_my_repair_requests = await scopedcontext.Add_User_Request
                        .Where(y => y.RequesterEmail == usermail).OrderByDescending(y=>y.CreatedOn).ToListAsync();

                    if (all_my_repair_requests == null)
                    {
                        return new BaseResponse { Code = "190", ErrorMessage = "nothing to show" };
                    }

                    return new BaseResponse { Code = "200", SuccessMessage="SUCCESSFULLY QUERIED" , Body=all_my_repair_requests};

                }
            }
            catch(Exception ex)
            {
                return new BaseResponse { Code = "120", ErrorMessage = ex.Message };
            }
        }



    }

}

        

