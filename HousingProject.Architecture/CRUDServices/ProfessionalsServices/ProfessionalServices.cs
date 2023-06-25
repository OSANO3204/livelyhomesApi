using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
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
        public ProfessionalServices(
            ILoggedIn loggedIn,
            HousingProjectContext context,
            IGenerateIdService generateIdService,
            IServiceScopeFactory servicescopefactory,
            ILoggedIn logged_in_user
            )
        {
            _loggedIn = loggedIn;
            _context = context;
            _generateIdService = generateIdService;
            _servicescopefactory = servicescopefactory;
            _logged_in_user = logged_in_user;
        }

        public async Task<BaseResponse> Createprofessonal(Professionalsvm vm)
        {
            var checkifexists = await _context.RegisterProfessional.Where(x => x.Email == vm.Email).FirstOrDefaultAsync();
            try
            {
                var user =  _logged_in_user.LoggedInUser().Result;
                var workid = _generateIdService.GenerateWorkId().Result.SuccessMessage;
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
                    User_Id= user.Id
                };


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
                    var scopedcontext = scope.ServiceProvider.GetRequiredService<HousingProjectContext>();
                    var professionalexists = await scopedcontext.RegisterProfessional.Where(y => y.User_Id == user_id).OrderByDescending(y=>y.DateCreated).ToListAsync();
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

                    var new_request = new Add_User_Request
                    {
                        Description = vm.Description,
                        Reason = vm.reason,
                        Job_Number = vm.Job_Number,
                        Worker_Email = vm.worker_Email,
                        Phone_Number=vm.Phone_Number,
                        Names=vm.Names
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

                    var allrequests = await scopedcontext.Add_User_Request.Where(y => y.Worker_Email == worker_email).ToListAsync();
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

                    var allrequests = await scopedcontext.Add_User_Request.Where(y => y.Job_Number == job_number).ToListAsync();
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


    }

}

        

