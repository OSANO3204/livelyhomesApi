using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HousingProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
                     
    }
}
