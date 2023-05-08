using HousingProject.Architecture.Data;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Professionals;
using HousingProject.Core.ViewModel.Professionalsvm;
using HousingProject.Infrastructure.ExtraFunctions.GenerateWorkId;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
using HousingProject.Infrastructure.Interfaces.IProfessionalsServices;
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
        public ProfessionalServices(
            ILoggedIn loggedIn,
            HousingProjectContext context,
            IGenerateIdService generateIdService,
            IServiceScopeFactory servicescopefactory)
        {
            _loggedIn = loggedIn;
            _context = context;
            _generateIdService = generateIdService;
            _servicescopefactory = servicescopefactory;
        }

        public async Task<BaseResponse> Createprofessonal(Professionalsvm vm)
        {
            var checkifexists = await _context.RegisterProfessional.Where(x => x.Email == vm.Email).FirstOrDefaultAsync();
            try
            {
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
                    JobNumber = workid
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
    }

    }

        

