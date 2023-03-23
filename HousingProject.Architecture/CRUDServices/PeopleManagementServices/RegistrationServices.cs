using HousingProject.Architecture.Data;
using HousingProject.Architecture.Interfaces.IEmail;
using HousingProject.Architecture.IPeopleManagementServvices;
using HousingProject.Architecture.Response.Base;
using HousingProject.Architecture.ViewModel.People;
using HousingProject.Core.Models.Email;
using HousingProject.Core.Models.People;
using HousingProject.Core.ViewModel.People.GeneralRegistration;
using HousingProject.Infrastructure.ExtraFunctions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Architecture.PeopleManagementServices
{
    public class RegistrationServices : IRegistrationServices
    {
        private readonly HousingProjectContext _context;
        private readonly IEmailServices _iemailservices;
        public ILogger<RegistrationServices> _logger;
        private byte[] keyByte;
        private readonly UserManager<RegistrationModel> usermanager;
        private readonly SignInManager<RegistrationModel> signinmanager;
        private readonly IverificationGenerator _iverificationGenerator;
        private readonly IHttpContextAccessor _httpContextAccesssor;



        public RegistrationServices(
                               HousingProjectContext context,
                               ILogger<RegistrationServices> logger,
                               UserManager<RegistrationModel> usermanager,
                               SignInManager<RegistrationModel> signinmanager,
                               IHttpContextAccessor httpContextAccesssor,
                               IEmailServices iemailservices,
                               IverificationGenerator iverificationGenerator

                               )


        {


            _context = context;
            _logger = logger;
            this.usermanager = usermanager;
            this.signinmanager = signinmanager;
            _httpContextAccesssor = httpContextAccesssor;
            _iemailservices = iemailservices;
            _iverificationGenerator = iverificationGenerator;
            // this.signnmanager = signnmanager;
        }



        // getloggedin user

        public async Task<RegistrationModel> LoggedInUser()
        {


            var currentuserid = _httpContextAccesssor.HttpContext.User.Claims.Where(x => x.Type == "Id").Select(p => p.Value).FirstOrDefault();
            var loggedinuser = await _context.RegistrationModel.Where(x => x.Id == currentuserid).FirstOrDefaultAsync();

            return loggedinuser;


        }
        //verify email

        public async Task<BaseResponse> AccountVerification(string verificationtoken)
        {
            try
            {
                if (verificationtoken == "")
                {

                    return new BaseResponse { Code = "980", ErrorMessage = "use the correct url for verification" };

                }
                var getuser = await _context.RegistrationModel.Where(x => x.VerificationToken == verificationtoken).FirstOrDefaultAsync();

                if (getuser == null)
                {
                    return new BaseResponse { Code = "940", ErrorMessage = "User does not exist" };

                }

                if (getuser.EmailConfirmed)
                {

                    return new BaseResponse { Code = "434", ErrorMessage = "Email already validated" };

                }
                getuser.EmailConfirmed = true;

                _context.Update(getuser);
                await _context.SaveChangesAsync();

                return new BaseResponse { Code = "200", SuccessMessage = "Email validated successfully" };

            }

            catch (Exception e)
            {


                return new BaseResponse { Code = "300", ErrorMessage = e.Message };
            }

        }


        public async Task<BaseResponse> UserRegistration(RegisterViewModel registervm)
        {
            // pycivaz @mailinator.com



            var verificationtoken = await _iverificationGenerator.GenerateToken();
            byte[] EmailInbytes = Encoding.ASCII.GetBytes(registervm.Email);
            ASCIIEncoding encoding = new ASCIIEncoding();
            var key = "hjgbaZZpAdzIWZvdypghyy6bvvWDh2GvHpTJPuA=";
            var keyByte = encoding.GetBytes(key);
            var hmacsha256 = new HMACSHA1(keyByte);
            var convertedtoken = verificationtoken.SuccessMessage + Convert.ToHexString(hmacsha256.ComputeHash(encoding.GetBytes(Convert.ToBase64String(EmailInbytes)))).ToLower();


            if (registervm.Password != registervm.RetypePassword)
            {

                return new BaseResponse { Code = "143", ErrorMessage = "Passwords do not match " };
            }
            var newuser = new RegistrationModel
            {

                FirstName = registervm.FirstName,
                LasstName = registervm.LasstName,
                BirthDate = registervm.BirthDate,
                Salutation = registervm.Salutation,
                Gender = registervm.Gender,
                Email = registervm.Email,
                IdNumber = registervm.IdNumber,
                PhoneNumber = registervm.PhoneNumber,
                UserName = registervm.Email,
                PasswordHash = registervm.Password,
                TenantId = registervm.TenantId,
                VerificationToken = convertedtoken,
                Is_Tenant = registervm.Is_Tenant,
                IsHouseUsers= registervm.IsHouseUsers



            };

            var createduser = await usermanager.CreateAsync(newuser, registervm.Password);

            if (createduser.Succeeded)
            {

                await signinmanager.SignInAsync(newuser, isPersistent: false);

                var sendbody = new UserEmailOptions
                {


                    UserName = convertedtoken,
                    PayLoad = "Welcome ",
                    ToEmail = newuser.Email,
                };

                await _iemailservices.EmailOnNewUserRegistrations(sendbody);

                return new BaseResponse { Code = "200", SuccessMessage = "User Created successfully" };

            }


            foreach (var error in createduser.Errors)
            {

                return new BaseResponse { Code = "100", ErrorMessage = error.Description };
            }




            //default message for anything else npt 
            return new BaseResponse { Code = "850", ErrorMessage = "Default  message" };

        }


        public async Task<IEnumerable> GetAllUsers()
        {
            return await _context.RegistrationModel.ToListAsync();
        }


        public async Task<BaseResponse> GetUserByUsername(string username)
        {
            var valuecheked = await _context.RegistrationModel
                .Where(x => x.UserName == username)
                .FirstOrDefaultAsync();
            if (valuecheked != null)
            {
                return new BaseResponse
                {
                    Code = "200",
                    SuccessMessage = "Query Successfull",
                    Body = valuecheked
                };
            }
            return (new BaseResponse { Code = "103", SuccessMessage = "User does not exist" });
        }


        public async Task<BaseResponse> AsigRole(AsignRoleviewModel vm)
        {
            if (vm.UserEmail == "")
            {

                return new BaseResponse { Code = "103", ErrorMessage = "Email cannot be empty" };
            }

            if (vm.userRole == "")
            {
                return new BaseResponse { Code = "104", ErrorMessage = "user role cannot be empty" };
            }

            var relateduser = await _context.RegistrationModel.Where(x => x.Email == vm.UserEmail).FirstOrDefaultAsync();
           
            if (vm.userRole == "Agent" && relateduser.Is_Agent)
            {

                return new BaseResponse { Code = "120", ErrorMessage = "user agent role already exist" };
            }
            if (vm.userRole == "CareTaker" && relateduser.Is_CareTaker)
            {

                return new BaseResponse { Code = "123", ErrorMessage = "user caretaker role already exist" };
            }
            if (vm.userRole == "Tenant" && relateduser.Is_Tenant)
            {

                return new BaseResponse { Code = "120", ErrorMessage = "user tenant role already exist" };
            }
            if (vm.userRole == "Landlord" && relateduser.Is_Landlord)
            {

                return new BaseResponse { Code = "120", ErrorMessage = "user landlord role already exist" };
            }

            if (vm.userRole == "Tenant" && relateduser.Is_Tenant)
            {

                return new BaseResponse { Code = "120", ErrorMessage = "user tenant role already exist" };
            }

            if (relateduser == null)
            {

                return new BaseResponse { Code = "106", ErrorMessage = "User does not exist" };
            }

            if (vm.userRole == "Admin" && relateduser.Is_Admin)
            {

                return new BaseResponse { Code = "120", ErrorMessage = "user admin role already exist" };
            }

            try
            {



                if (relateduser != null)
                {

                    if (vm.userRole == "Agent")
                    {


                        relateduser.Is_Agent = true;

                        _context.Update(relateduser);
                        await _context.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "User role update to agent" };


                    }

                    if (vm.userRole == "Admin")
                    {
                        relateduser.Is_Admin = true;
                        relateduser.Is_Agent = true;
                        relateduser.Is_Tenant = false;
                        relateduser.Is_Landlord = true;
                        relateduser.IsHouseUsers = false;
                        relateduser.Is_CareTaker= true;
                        _context.Update(relateduser);
                        await _context.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "User role update to Admin" };

                    }

                    if (vm.userRole == "Landlord")
                    {
                        relateduser.Is_Admin = false;
                        relateduser.Is_Agent = false;
                        relateduser.Is_Tenant = false;                       
                        relateduser.IsHouseUsers = false;
                        relateduser.Is_CareTaker = false;
                        relateduser.Is_Landlord = true;
                        _context.Update(relateduser);
                        await _context.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "User role update to Landlord" };

                    }
                    if (vm.userRole == "CareTaker")
                    {
                        relateduser.Is_Admin = false;
                        relateduser.Is_Agent = false;
                        relateduser.Is_Tenant = false;
                        relateduser.IsHouseUsers = false;

                        relateduser.Is_CareTaker = true;
                        _context.Update(relateduser);
                        await _context.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "User role update to Caretaker" };

                    }

                    if (vm.userRole == "Tenant")
                    {
                        relateduser.Is_Admin = false;
                        relateduser.Is_Agent = false;
                    
                        relateduser.IsHouseUsers = false;
                        relateduser.Is_CareTaker = false;
                        relateduser.Is_Tenant = true;
                        _context.Update(relateduser);
                        await _context.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "User role update to Tenant" };

                    }



                    return new BaseResponse { Code = "99", ErrorMessage = "something happened" };


                }
            }

            catch (Exception e)
            {

                return new BaseResponse { Code = "101", ErrorMessage = e.Message };

            }

            return new BaseResponse { };


        }



        //remove role
        public async Task<BaseResponse> RomveRole(AsignRoleviewModel vm)
        {


            if (vm.UserEmail == "")
            {

                return new BaseResponse { Code = "103", ErrorMessage = "Email cannot be empty" };
            }

            if (vm.userRole == "")
            {
                return new BaseResponse { Code = "104", ErrorMessage = "user role cannot be empty" };
            }

            var relateduser = await _context.RegistrationModel.Where(x => x.Email == vm.UserEmail).FirstOrDefaultAsync();


            if (relateduser == null)
            {

                return new BaseResponse { Code = "106", ErrorMessage = "User does not exist" };
            }

            try
            {



                if (relateduser != null)
                {
                    if (vm.userRole == "Admin" && relateduser.Is_Admin)
                    {


                        relateduser.Is_Agent = false;
                        relateduser.Is_Admin = false;
                        relateduser.Is_CareTaker=false;
                        relateduser.Is_Landlord= false;
                        relateduser.IsHouseUsers = false;
                        relateduser.Is_Tenant=false;


                        _context.Update(relateduser);
                        await _context.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "agent role remove" };


                    }

                    if (vm.userRole == "Agent" && relateduser.Is_Agent)
                    {


                        relateduser.Is_Agent = false;

                        _context.Update(relateduser);
                        await _context.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "agent role remove" };


                    }

                    if (vm.userRole == "Landlord" && relateduser.Is_Landlord)
                    {
                        relateduser.Is_Landlord = false;
                        _context.Update(relateduser);
                        await _context.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "Landlord role removed" };

                    }
                    if (vm.userRole == "CareTaker" && relateduser.Is_CareTaker)
                    {
                        relateduser.Is_CareTaker = false;
                        _context.Update(relateduser);
                        await _context.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "Caretaker role removed" };

                    }

                    if (vm.userRole == "Tenant" && relateduser.Is_Tenant)
                    {
                        relateduser.Is_Tenant = false;
                        _context.Update(relateduser);
                        await _context.SaveChangesAsync();
                        return new BaseResponse { Code = "200", SuccessMessage = "Tenant role removed" };

                    }


                    return new BaseResponse { Code = "99", ErrorMessage = "No role to remove" };


                }
            }

            catch (Exception e)
            {

                return new BaseResponse { Code = "101", ErrorMessage = e.Message };

            }

            return new BaseResponse { };


        }

    }


}
