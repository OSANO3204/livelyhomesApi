using HousingProject.Architecture.Data;
using HousingProject.Architecture.Interfaces.IEmail;
using HousingProject.Architecture.Interfaces.IlogginServices;
using HousingProject.Architecture.Response.Base;
using HousingProject.Core.Models.Email;
using HousingProject.Core.Models.People;
using HousingProject.Core.Models.People.General;
using HousingProject.Core.Models.Reply;
using HousingProject.Core.ViewModel.Passwords;
using HousingProject.Core.ViewModel.People.GeneralRegistration;
using HousingProject.Core.ViewModel.Resplyvm;
using HousingProject.Infrastructure.ExtraFunctions.LoggedInUser;
using HousingProject.Infrastructure.Response.ReplyResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Architecture.Services.User_Login
{
    public class UserLoginServices : IloggedInServices
    {
        private readonly HousingProjectContext _context;
        private readonly UserManager<RegistrationModel> usermanager;
        private readonly SignInManager<RegistrationModel> signinmanager;
        private readonly IEmailServices _iemailservvices;
        private readonly IHttpContextAccessor _httpcontextaccessor;
        private readonly ILogger<UserLoginServices> _ilogger;
        private readonly IServiceScopeFactory _servicefactory;
        private readonly ILoggedIn _loggedinuser;
        public UserLoginServices(
            HousingProjectContext context,
            SignInManager<RegistrationModel> signinmanager,
            UserManager<RegistrationModel> usermanager,
            ILogger<UserLoginServices> ilogger,
            IHttpContextAccessor httpcontextaccessor,
            IEmailServices iemailServices,
            IServiceScopeFactory servicefactory,
            ILoggedIn loggedinuser
        )
        {
            _context = context;
            this.usermanager = usermanager;
            this.signinmanager = signinmanager;
            _httpcontextaccessor = httpcontextaccessor;
            _ilogger = ilogger;
            _iemailservvices = iemailServices;
            _servicefactory = servicefactory;
            _loggedinuser = loggedinuser;
        }

        public async Task<RegistrationModel>
        ValidateUser(UserLogin credentials)
        {

            var identityUser =
                await usermanager.FindByEmailAsync(credentials.UserName);
            if (identityUser != null)
            {
                var result =
                    usermanager
                        .PasswordHasher
                        .VerifyHashedPassword(identityUser,
                        identityUser.PasswordHash,
                        credentials.Password);

                return result == PasswordVerificationResult.Failed
                    ? null
                    : identityUser;
            }

            return null;
        }
        public async Task<authenticationResponses>   Authenticate(UserLogin loggedinuser)
        {
            try
            {
                //general auths
                if (loggedinuser.UserName == "")
                {
                    return (
                    new authenticationResponses
                    {
                        Code = "10",
                        ErrorMessage = "username cannot be empty"
                    }
                    );
                }
                if (loggedinuser.Password == "")
                {
                    return (
                    new authenticationResponses
                    {
                        Code = "11",
                        ErrorMessage = "Password cannot  be null"
                    }
                    );
                }
                var identityUser = await usermanager.FindByEmailAsync(loggedinuser.UserName);

                if (identityUser == null)
                {
                    return new authenticationResponses { Code = "105", ErrorMessage = "The user doesnt exist" };
                }

                if (!identityUser.EmailConfirmed)
                {
                    return new authenticationResponses { Code = "123", ErrorMessage = "Kindly  check   your email  to validate your account first" };
                }
                if (identityUser != null)
                {
                    var result =
                        usermanager
                            .PasswordHasher
                            .VerifyHashedPassword(identityUser,
                            identityUser.PasswordHash,
                            loggedinuser.Password);

                    if (result == PasswordVerificationResult.Failed)
                    {
                        return new authenticationResponses
                        {
                            Code = "920",
                            ErrorMessage = "Wrong login details"
                        };
                    }

                    var userexists =
                        await _context
                            .RegistrationModel
                            .Where(x => x.UserName == loggedinuser.UserName)
                            .FirstOrDefaultAsync();

                    if (userexists == null)
                    {
                        return
                        new authenticationResponses
                        {
                            Code = "103",
                            ErrorMessage = "User does not  exist"
                        };

                    }

                    var tokenexpirytimestamp =
                        DateTime
                            .Now
                            .AddMinutes(Constants.Constants.JWT_TOKEN_VALIDITY);
                    var jwtsecuritytokenhandler = new JwtSecurityTokenHandler();
                    var tokenkey =
                        Encoding
                            .ASCII
                            .GetBytes(Constants.Constants.JWT_SECURITY_KEY);
                    var securitytokendescripor =
                        new SecurityTokenDescriptor
                        {
                            Subject =
                                new ClaimsIdentity(new List<Claim> {
                                    new Claim("Username",
                                        loggedinuser.UserName),
                                    new Claim("FirstName",
                                        identityUser.FirstName),
                                    new Claim("LastName",
                                        identityUser.LasstName),
                                    new Claim("Id", identityUser.Id)
                                    }),
                            Expires = tokenexpirytimestamp,
                            SigningCredentials =
                                new SigningCredentials(new SymmetricSecurityKey(tokenkey),
                                    SecurityAlgorithms.HmacSha256Signature)
                        };

                    var securitytoken =
                        jwtsecuritytokenhandler.CreateToken(securitytokendescripor);
                    var token = jwtsecuritytokenhandler.WriteToken(securitytoken);
                    return new authenticationResponses
                    {
                        Code = "200",
                        SuccessMessage = "Logged in successfully",
                        FirstName = userexists.FirstName,
                        Lastname = userexists.LasstName,
                        token = token,
                        Username = userexists.Email,
                        IdNumber = userexists.IdNumber,
                        Is_Agent = userexists.Is_Agent,
                        Is_CareTaker = userexists.Is_CareTaker,
                        Is_Landlord = userexists.Is_Landlord,
                        ExpiryTime = (int)tokenexpirytimestamp.Subtract(DateTime.Now).TotalSeconds
                    };
                }

            }

            catch (Exception e)
            {
                _ilogger.LogInformation("Error message on login : ", e.Message);
                return new authenticationResponses
                {
                    Code = "880",
                    ErrorMessage = e.Message
                };

            }

            return new authenticationResponses { };

        }


        //start
        public async Task<BaseResponse> ContactUs(ContactUsViewModel vm)
        {
            try
            {
                var loggedinuser = LoggedInUser().Result;
                if (vm.UserMessage == null)
                {

                    return new BaseResponse { Code = "111", ErrorMessage = "Message cannot be empty" };
                }

                var contactbody = new ContactUs()
                {
                   Message_title =vm.Message_title,
                    Useremail = loggedinuser.Email,
                    UserMessage = vm.UserMessage

                };

                await _context.AddAsync(contactbody);
                await _context.SaveChangesAsync();
             

                var sendbody = new UserEmailOptions
                {
                    UserName = loggedinuser.FirstName,
                    PayLoad = "sent mail test",
                    ToEmail = loggedinuser.Email
                };

                var resp = await _iemailservvices.OnContusMessageSubmission(sendbody);

                if (resp.Code == "200")
                {

                    return new BaseResponse { Code = "200", SuccessMessage = "Received sucessfully, " +
                        "  Lively home" +
                        $"  will contact you shortly  through your email {loggedinuser.Email} " };
                }

                else
                {
                    return new BaseResponse
                    {

                        Code = "104",
                        SuccessMessage = "Message  sent but email failed to your inbox, make sure you are not blocking emails"
                    };

                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { Code = "120", ErrorMessage = ex.Message };

            }
        }
        //end   
        public async Task<BaseResponse> ResetPassword(ResetPassword resetpasswordvm)
        {

            if (resetpasswordvm.Password != resetpasswordvm.RetypePassword)
            {

                return new BaseResponse { Code = "140", ErrorMessage = "Passwords should always be the same" };
            }
        ;
            var getuserbyusername =
                await _context
                    .RegistrationModel
                    .Where(x => x.Email == resetpasswordvm.Email)
                    .FirstOrDefaultAsync();

            if (getuserbyusername == null)
            {
                return new BaseResponse
                {
                    Code = "105",
                    ErrorMessage = "Username does not exist"
                };
            }
            if (resetpasswordvm.Email != "")
            {
                if (resetpasswordvm.Password == getuserbyusername.PasswordHash)
                {
                    return new BaseResponse
                    {
                        Code = "106",
                        ErrorMessage =
                            "Please choose a password you haven't used before"
                    };
                }

                if (getuserbyusername != null)
                {
                    getuserbyusername.PasswordHash = resetpasswordvm.Password;

                    getuserbyusername.PasswordHash =
                        usermanager
                            .PasswordHasher
                            .HashPassword(getuserbyusername,
                            resetpasswordvm.Password);

                    await usermanager.UpdateAsync(getuserbyusername);

                    _context.SaveChanges();
                }
            }
            return new BaseResponse
            {
                Code = "200",
                SuccessMessage = "Password reset Successfully!"
            };
        }

        public async Task<RegistrationModel> LoggedInUser()
        {
            var currentuserid =
                _httpcontextaccessor
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
        
        public async Task<BaseResponse> GetUserroles()
        {


            var loggedinuser = LoggedInUser().Result;

            if (loggedinuser.Is_Admin)
            {

                return new BaseResponse { Code = "200", SuccessMessage = "Administrator " };
            }

            if (loggedinuser.Is_Landlord && loggedinuser.Is_CareTaker && loggedinuser.Is_Agent)
            {

                return new BaseResponse { Code = "200", SuccessMessage = "Supervisor" };
            }

            else if (loggedinuser.Is_Landlord && loggedinuser.Is_Agent)
            {
                return new BaseResponse { Code = "200", SuccessMessage = "Landlord and Agent" };
            }
            else if (loggedinuser.Is_Landlord && loggedinuser.Is_CareTaker)
            {
                return new BaseResponse { Code = "200", SuccessMessage = "Landlord and Caretaker" };
            }
            else if (loggedinuser.Is_CareTaker && loggedinuser.Is_Agent)
            {
                return new BaseResponse { Code = "200", SuccessMessage = "Caretaker and Agent" };
            }
            else if (loggedinuser.Is_CareTaker && loggedinuser.Is_Tenant)
            {
                return new BaseResponse { Code = "200", SuccessMessage = "Tenant and Caretaker" };
            }

            else if (loggedinuser.Is_Tenant && loggedinuser.Is_Landlord)
            {
                return new BaseResponse { Code = "200", SuccessMessage = "Landlord and Tenant" };
            }

            else if (loggedinuser.Is_Tenant && loggedinuser.Is_Agent)
            {
                return new BaseResponse { Code = "200", SuccessMessage = "Agent and Tenant" };
            }
            else if (loggedinuser.Is_CareTaker)
            {
                return new BaseResponse { Code = "200", SuccessMessage = "Caretaker" };
            }

            else if (loggedinuser.Is_Landlord)
            {

                return new BaseResponse { Code = "200", SuccessMessage = "Landlord" };
            }

            else if (loggedinuser.Is_Admin)
            {

                return new BaseResponse { Code = "200", SuccessMessage = "Admin" };
            }



            else if (loggedinuser.Is_Agent)
            {

                return new BaseResponse { Code = "200", SuccessMessage = "Agent" };
            }

            else if (loggedinuser.Is_Tenant)
            {
                return new BaseResponse { Code = "200", SuccessMessage = "Tenant" };

            }
            else
            {

                return new BaseResponse { Code = "1050", ErrorMessage = "You Have No role" };
            }
        }


        public async Task<BaseResponse> ChangeLastName(string LastName)
        {
            try
            {
                var currentuser = LoggedInUser().Result;

                if (currentuser.LasstName != LastName)
                {
                    currentuser.LasstName = LastName;
                    _context.RegistrationModel.Update(currentuser);
                    await _context.SaveChangesAsync();
                }

                return new BaseResponse
                {
                    Code = "200",
                    SuccessMessage = "Name changed successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Code = "200",
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<BaseResponse> ChangeFirstName(string FirstName)
        {
            try
            {
                var currentuser = LoggedInUser().Result;

                if (currentuser.FirstName != FirstName)
                {
                    currentuser.FirstName = FirstName;
                    _context.RegistrationModel.Update(currentuser);
                    await _context.SaveChangesAsync();
                }

                return new BaseResponse
                {
                    Code = "200",
                    SuccessMessage = "Name changed successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Code = "200",
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<BaseResponse> ChangeUserEmail(string emailaddress)
        {
            try
            {
                var currentuser = LoggedInUser().Result;

                if (currentuser.Email != emailaddress)
                {
                    currentuser.Email = emailaddress;
                    currentuser.NormalizedEmail = emailaddress;
                    currentuser.UserName = emailaddress;
                    currentuser.NormalizedUserName = emailaddress;

                    _context.RegistrationModel.Update(currentuser);
                    await _context.SaveChangesAsync();
                }

                return new BaseResponse
                {
                    Code = "200",
                    SuccessMessage = "Email changed successfully "
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Code = "350",
                    ErrorMessage = ex.Message
                };
            }
        }

      
    }
}
