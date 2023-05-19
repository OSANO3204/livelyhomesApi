using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingProject.Infrastructure.Response.VotesResponse
{
 public    class VotesResponse
    {
        public string Code { get; set; }
        public string  Message { get; set; }
        public decimal TotalVotes { get; set; }
        public decimal UpVotes { get; set; }
        public decimal DownVotes { get; set; }
        public double UserRating { get; set; }


        public VotesResponse(string code, string message, decimal totavotes, decimal upvotes, decimal downvotes, double userrating)
        {
            Code = code;
            Message = message;
            TotalVotes = totavotes;
            UpVotes = upvotes;
            DownVotes = downvotes;
            UserRating = userrating;
        }
    }
}
