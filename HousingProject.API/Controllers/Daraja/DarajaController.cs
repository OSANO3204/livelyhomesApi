using HousingProject.Infrastructure.Interfaces.IDarraja;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousingProject.API.Controllers.Daraja
{

    [Route("api/[controller]", Name = "Payment")]
    [ApiController]
    public class DarajaController : IDarajaServices
    {
        private readonly IDarajaServices _darajaServices;
        public DarajaController(IDarajaServices darajaServices)
        {
            _darajaServices = darajaServices;
        }

      
        [Route("Getting_auth_token")]
        [HttpGet]
        public Task<string> FetchAccessToken()
        {
            return  _darajaServices.FetchAccessToken();
        }
    }
}
