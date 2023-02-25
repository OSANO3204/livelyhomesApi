using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Response
{
    public class imageUploadResponse
    {

        public string Name { get; set; }
        public string ImageBase64 { get; set; }

        public byte[] filebyte { get; set; }

    }
}
