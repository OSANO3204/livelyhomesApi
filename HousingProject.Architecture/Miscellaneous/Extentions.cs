using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Architecture.Miscellaneous
{
   public  class Extentions
    {

        public async  static Task<string>GenerateRandoms()
        {
            Random RNG = new Random();
            const string range = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var chars = Enumerable.Range(0, 10).Select(x => range[RNG.Next(0, range.Length)]);
            return new string(chars.ToArray());
        }
    }
}
