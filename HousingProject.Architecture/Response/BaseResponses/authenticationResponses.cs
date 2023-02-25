using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Architecture.Response.Base
{
  public   class authenticationResponses
    {
        public string Code { get; set; }

        public string  SuccessMessage { get; set; }

        public string ErrorMessage { get; set; }

        public string token { get; set; }

        public string Username { get; set; }

        public int ExpiryTime { get; set; }

        public  string  Email{ get; set; }

        public int UserId { get; set; }

        public string IdNumber { get; set; }

        public string  FirstName { get; set; }

        public string Lastname { get; set; }

        public bool Is_Agent { get; set; }

        public bool Is_CareTaker { get; set; }

        public bool Is_Landlord { get; set; }





    }
}
