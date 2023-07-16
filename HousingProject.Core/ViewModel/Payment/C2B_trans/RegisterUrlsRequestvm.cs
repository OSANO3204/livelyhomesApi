using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Core.ViewModel.Payment.C2B_trans
{
    public class RegisterUrlsRequestvm
    {
        [JsonProperty("ValidationUrl")]
        public string ValidationUrl { get; set; }

        [JsonProperty("ConfirmationUrl")]
        public string ConfirmationUrl { get; set; }

        [JsonProperty("ShortCode")]
        public string ShortCode { get; set; }

        [JsonProperty("ResponseType")]
        public string ResponseType { get; set; }
    }
}
