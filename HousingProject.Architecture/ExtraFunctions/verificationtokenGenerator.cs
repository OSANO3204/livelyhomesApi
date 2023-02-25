using HousingProject.Architecture.Response.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.ExtraFunctions
{
    public class verificationtokenGenerator: IverificationGenerator
    {
        private static readonly ASCIIEncoding encoding = new ASCIIEncoding();
        public  async Task<BaseResponse>    GenerateToken()
        {
            Random rand =  new Random();

            try
            {
                int stringlen = rand.Next(4, 10);
                int randValue;
                string str = "";
                string finalencoded = "";
                char letter;
                for (int i = 0; i < stringlen; i++)
                {

                    randValue = rand.Next(0, 26);


                    letter = Convert.ToChar(randValue + 65);


                    str = str + letter;



                    
                }
                var key = "aZZpAdzIWZvdypWDh2GvHpTJPuA=";
                var message = str;
                var keyByte = encoding.GetBytes(key)
;
                using (var hmacsha256 = new HMACSHA1(keyByte))
                {
                    byte[] messagetobyte = encoding.GetBytes(message);
                    var messagetobase64 = Convert.ToBase64String(messagetobyte);
                    var getbitesfrombase64string = encoding.GetBytes(messagetobase64);
                    hmacsha256.ComputeHash(getbitesfrombase64string);
                    var hashedvalue = Convert.ToHexString(hmacsha256.Hash);
                    var hashedtolowercase = hashedvalue.ToLower();
                    Console.WriteLine(hashedtolowercase);

                    finalencoded = hashedtolowercase;
                }
                return new BaseResponse { Code = "200", SuccessMessage = finalencoded };

              


            }
            catch (Exception e)
            {

                return new BaseResponse { Code = "409", ErrorMessage = e.Message };
            }


        }
    }
}
